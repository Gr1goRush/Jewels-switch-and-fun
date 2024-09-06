using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace MatchThreeEngine
{
	public sealed class Board : MonoBehaviour
	{
		[SerializeField] private TileTypeAsset[] tileTypes;

		[SerializeField] private Row[] rows;

		//[SerializeField] private AudioClip matchSound;

		//[SerializeField] private AudioSource audioSource;

		[SerializeField] private float tweenDuration;

		[SerializeField] private Transform swappingOverlay;

		[SerializeField] private bool ensureNoStartingMatches;

		private readonly List<Tile> _selection = new List<Tile>();

		private bool _isSwapping;
		private bool _isMatching;
		private bool _isShuffling;

		public event Action<TileTypeAsset, int> OnMatch;

		private TileData[,] Matrix
		{
			get
			{
				var width = rows.Max(row => row.tiles.Length);
				var height = rows.Length;

				var data = new TileData[width, height];

				for (var y = 0; y < height; y++)
					for (var x = 0; x < width; x++)
						data[x, y] = GetTile(x, y).Data;

				return data;
			}
		}

		private void Start()
		{
			for (var y = 0; y < rows.Length; y++)
			{
				for (var x = 0; x < rows.Max(row => row.tiles.Length); x++)
				{
					var tile = GetTile(x, y);

					tile.x = x;
					tile.y = y;

					tile.Type = tileTypes[Random.Range(0, tileTypes.Length)];

					tile.button.onClick.AddListener(() => Select(tile));
				}
			}

			if (ensureNoStartingMatches) StartCoroutine(EnsureNoStartingMatches());

			OnMatch += (type, count) => Debug.Log($"Matched {count}x {type.name}.");
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				var bestMove = TileDataMatrixUtility.FindBestMove(Matrix);

				if (bestMove != null)
				{
					Select(GetTile(bestMove.X1, bestMove.Y1));
					Select(GetTile(bestMove.X2, bestMove.Y2));
				}
			}
		}

		private IEnumerator EnsureNoStartingMatches()
		{
			var wait = new WaitForEndOfFrame();

			while (TileDataMatrixUtility.FindBestMatch(Matrix) != null)
			{
				Shuffle();

				yield return wait;
			}
		}

		private Tile GetTile(int x, int y) => rows[y].tiles[x];

		private Tile[] GetTiles(IList<TileData> tileData)
		{
			var length = tileData.Count;

			var tiles = new Tile[length];

			for (var i = 0; i < length; i++) tiles[i] = GetTile(tileData[i].X, tileData[i].Y);

			return tiles;
		}

		private async void Select(Tile tile)
		{
			if (_isSwapping || _isMatching || _isShuffling) return;

            if (!_selection.Contains(tile))
            {
                if (_selection.Count > 0)
                {
                    if (Math.Abs(tile.x - _selection[0].x) == 1 && Math.Abs(tile.y - _selection[0].y) == 0
                        || Math.Abs(tile.y - _selection[0].y) == 1 && Math.Abs(tile.x - _selection[0].x) == 0)
                        _selection.Add(tile);
                    GameCounter.Instance.Step += 1;////saving step
                }
                else
                {
                    _selection.Add(tile);
                }
            }

            if (_selection.Count < 2) return;

			await SwapAsync(_selection[0], _selection[1]);

			if (!await TryMatchAsync()) await SwapAsync(_selection[0], _selection[1]);
			//else ScoreCounter.Instance.Score += _selection.Count * _selection[1].Type.value;////saving score

            var matrix = Matrix;

			while (TileDataMatrixUtility.FindBestMove(matrix) == null || TileDataMatrixUtility.FindBestMatch(matrix) != null)
			{
				Shuffle();

				matrix = Matrix;
			}

			_selection.Clear();
		}

		private async Task SwapAsync(Tile tile1, Tile tile2)
		{
			_isSwapping = true;

			var icon1 = tile1.icon;
			var icon2 = tile2.icon;

			var icon1Transform = icon1.transform;
			var icon2Transform = icon2.transform;

			icon1Transform.SetParent(swappingOverlay);
			icon2Transform.SetParent(swappingOverlay);

			icon1Transform.SetAsLastSibling();
			icon2Transform.SetAsLastSibling();

			var sequence = DOTween.Sequence();

			sequence.Join(icon1Transform.DOMove(icon2Transform.position, tweenDuration).SetEase(Ease.OutBack))
					.Join(icon2Transform.DOMove(icon1Transform.position, tweenDuration).SetEase(Ease.OutBack));

			await sequence.Play()
						  .AsyncWaitForCompletion();

			icon1Transform.SetParent(tile2.transform);
			icon2Transform.SetParent(tile1.transform);

			tile1.icon = icon2;
			tile2.icon = icon1;

			var tile1Item = tile1.Type;

			tile1.Type = tile2.Type;

			tile2.Type = tile1Item;

			_isSwapping = false;
		}

		private async Task<bool> TryMatchAsync()
		{
			var didMatch = false;

			_isMatching = true;

			var match = TileDataMatrixUtility.FindBestMatch(Matrix);

			while (match != null)
			{
				didMatch = true;

				var tiles = GetTiles(match.Tiles);

				var deflateSequence = DOTween.Sequence();

				foreach (var tile in tiles) deflateSequence.Join(tile.icon.transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack));

				AudioManager.Instance.PlaySFX("Clear");
				//audioSource.PlayOneShot(matchSound);
				//audioSource.volume = PlayerPrefs.GetFloat("_MusicValue", 1f);

				await deflateSequence.Play()
									 .AsyncWaitForCompletion();

                GameCounter.Instance.Score += _selection.Count * _selection[1].Type.value;////////////

                var inflateSequence = DOTween.Sequence();

				foreach (var tile in tiles)
				{
					tile.Type = tileTypes[Random.Range(0, tileTypes.Length)];

					inflateSequence.Join(tile.icon.transform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack));
				}

				await inflateSequence.Play()
									 .AsyncWaitForCompletion();

				OnMatch?.Invoke(Array.Find(tileTypes, tileType => tileType.id == match.TypeId), match.Tiles.Length);

				match = TileDataMatrixUtility.FindBestMatch(Matrix);
			}

			_isMatching = false;

			return didMatch;
		}

		private void Shuffle()
		{
			_isShuffling = true;

			foreach (var row in rows)
				foreach (var tile in row.tiles)
					tile.Type = tileTypes[Random.Range(0, tileTypes.Length)];

			_isShuffling = false;
		}

		/////////

		private async Task ExplodeBombAsync(int x, int y)
		{
			var explosionTiles = new List<Tile>();

			for (var i = -1; i <= 1; i++)
			{
				for (var j = -1; j <= 1; j++)
				{
					var newX = x + i;
					var newY = y + j;

					if (newX >= 0 && newX < rows[0].tiles.Length && newY >= 0 && newY < rows.Length)
					{
						var tile = GetTile(newX, newY);
						if (tile != null)
							explosionTiles.Add(tile);
					}
				}
			}

			var explosionSequence = DOTween.Sequence();

			foreach (var tile in explosionTiles)
			{
				explosionSequence.Join(tile.icon.transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack));
				// Здесь можно добавить дополнительные эффекты для визуализации взрыва
			}

			await explosionSequence.Play().AsyncWaitForCompletion();

			foreach (var tile in explosionTiles)
			{
				tile.Type = tileTypes[Random.Range(0, tileTypes.Length)];
				tile.icon.transform.localScale = Vector3.one;
			}
		}

		private async Task UseMagicWandAsync(int x, int y)
		{
			var horizontalLine = new List<Tile>();
			var verticalLine = new List<Tile>();

			for (var i = 0; i < rows.Length; i++)
			{
				var tileHorizontal = GetTile(i, y);
				if (tileHorizontal != null)
					horizontalLine.Add(tileHorizontal);

				var tileVertical = GetTile(x, i);
				if (tileVertical != null)
					verticalLine.Add(tileVertical);
			}

			var magicSequence = DOTween.Sequence();

			foreach (var tile in horizontalLine.Concat(verticalLine))
			{
				magicSequence.Join(tile.icon.transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack));
				// Здесь можно добавить дополнительные эффекты для визуализации магии
			}

			await magicSequence.Play().AsyncWaitForCompletion();

			foreach (var tile in horizontalLine.Concat(verticalLine))
			{
				tile.Type = tileTypes[Random.Range(0, tileTypes.Length)];
				tile.icon.transform.localScale = Vector3.one;
			}
		}

		public async Task RefreshBoardAsync()
		{
			_isShuffling = true;

			var refreshSequence = DOTween.Sequence();

			foreach (var row in rows)
			{
				foreach (var tile in row.tiles)
				{
					refreshSequence.Join(tile.icon.transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack));
					// Здесь можно добавить дополнительные эффекты для визуализации обновления поля
				}
			}

			await refreshSequence.Play().AsyncWaitForCompletion();

			foreach (var row in rows)
			{
				foreach (var tile in row.tiles)
				{
					tile.Type = tileTypes[Random.Range(0, tileTypes.Length)];
					tile.icon.transform.localScale = Vector3.one;
				}
			}

			_isShuffling = false;
		}

        /////////
       
        public async void UseBomb()
        {
            int bombX = 3; 
            int bombY = 3; 
            await ExplodeBombAsync(bombX, bombY);
			AudioManager.Instance.PlaySFX("Bomb");
        }
     
        public async void UseFirework()
        {
            int wandX = 5;
            int wandY = 2; 
            await UseMagicWandAsync(wandX, wandY);
            AudioManager.Instance.PlaySFX("Firework");
        }

        public async void UseMagicWand()
        {
            await RefreshBoardAsync();
            AudioManager.Instance.PlaySFX("MagicWand");
        }
    }
}
