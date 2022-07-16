using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Cards
{
    public abstract class AbstractCard : MonoBehaviour
    {
        [SerializeField] private GameObject decisionCanvasPrefab;
        
        private TextMeshProUGUI _costText;
        private TextMeshProUGUI _descriptionText;
        private TextMeshProUGUI _diceText;
        private TextMeshProUGUI _flavorText;
        private TextMeshProUGUI _titleText;

        private SpriteRenderer _foreGround;
        private SpriteRenderer _art;
        private Canvas _canvas;

        private bool _scaleUpCoroutineIsRunning = false;
        private bool _scaleDownCoroutineIsRunning = false;
        private bool _scaledUp = false;
        private bool _shouldResize = true;
        private bool _usable = false;
        private bool _isCurrentlyDragging = false;

        private Vector3 _positionInCardBank;

        private GameObject _decisionCanvas;

        public bool Usable
        {
            get => _usable;
            set => _usable = value;
        }

        public bool ShouldResize
        {
            get => _shouldResize;
            set => _shouldResize = value;
        }

        private void Start()
        {
            foreach (var box in transform.GetComponentsInChildren<TextMeshProUGUI>())
            {
                var textName = box.name;

                if (textName.Contains("cost", StringComparison.CurrentCultureIgnoreCase))
                    _costText = box;

                if (textName.Contains("name", StringComparison.CurrentCultureIgnoreCase))
                    _titleText = box;

                if (textName.Contains("description", StringComparison.CurrentCultureIgnoreCase))
                    _descriptionText = box;

                if (textName.Contains("dice", StringComparison.CurrentCultureIgnoreCase))
                    _diceText = box;

                if (textName.Contains("flavor", StringComparison.CurrentCultureIgnoreCase))
                    _flavorText = box;
            }

            UpdateCostText(GetCardData().Costs.ToString());
            UpdateNameText(GetCardData().CardName);
            UpdateDescriptionText(GetCardData().Description);
            UpdateDiceText(GetCardData().DiceText);
            UpdateFlavorText(GetCardData().FlavorText);
        }

        public void AdjustOrderIndex(int offset)
        {
            if (_canvas == null || _art == null || _foreGround == null)
            {
                foreach (var childRenderer in transform.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (childRenderer.gameObject.name.Contains("foreground", StringComparison.CurrentCultureIgnoreCase))
                        _foreGround = childRenderer;

                    if (childRenderer.gameObject.name.Contains("art", StringComparison.CurrentCultureIgnoreCase))
                        _art = childRenderer;
                }

                _canvas = GetComponentInChildren<Canvas>();
            }

            _art.sortingOrder = 1 + offset;
            _foreGround.sortingOrder = 2 + offset;
            _canvas.sortingOrder = 3 + offset;
        }

        public void UpdateCostText(string text)
        {
            _costText.text = text;
        }

        public void UpdateNameText(string text)
        {
            _titleText.text = text;
        }

        public void UpdateDescriptionText(string text)
        {
            _descriptionText.text = text;
        }

        public void UpdateDiceText(string text)
        {
            _diceText.text = text;
        }

        public void UpdateFlavorText(string text)
        {
            _flavorText.text = text;
        }

        private void OnMouseDrag()
        {
            if (!Usable)
            {
                return;
            }

            if (!_isCurrentlyDragging)
            {
                _isCurrentlyDragging = true;
                transform.localScale = new Vector3(.5f, .5f, 1);

                _decisionCanvas = Instantiate(decisionCanvasPrefab);
            }
            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            transform.position = pos;
        }

        private void OnMouseUp()
        {
            transform.position = _positionInCardBank;
            transform.localScale = new Vector3(.5f, .5f, 1);
            _isCurrentlyDragging = false;

            if (_decisionCanvas != null)
            {
                Destroy(_decisionCanvas);
            }
        }


        private void OnMouseOver()
        {
            if (_scaleDownCoroutineIsRunning && _shouldResize)
            {
                StopCoroutine("ScaleDown");
                _scaleDownCoroutineIsRunning = false;
            }

            if (!_scaleUpCoroutineIsRunning && !_scaledUp)
                StartCoroutine("ScaleUp");
        }

        private void OnMouseExit()
        {
            if (_scaleUpCoroutineIsRunning)
            {
                StopCoroutine("ScaleUp");
                _scaleUpCoroutineIsRunning = false;
            }

            StartCoroutine("ScaleDown");
        }

        private IEnumerator ScaleDown()
        {
            _scaleDownCoroutineIsRunning = true;

            for (int i = 0; i < 10; i++)
            {
                if (transform.localScale.x <= .5)
                    break;

                transform.localScale = new Vector3(
                    Mathf.Lerp(transform.localScale.x, transform.localScale.x - 0.02f, Mathf.SmoothStep(0f, 1f, i)),
                    Mathf.Lerp(transform.localScale.y, transform.localScale.y - 0.02f, Mathf.SmoothStep(0f, 1f, i)),
                    1
                );

                transform.position = new Vector3(
                    transform.position.x,
                    Mathf.Lerp(transform.position.y, transform.position.y - 0.06f, Mathf.SmoothStep(0f, 1f, i)),
                    transform.position.z
                );

                yield return new WaitForSeconds(0.01f);
            }

            _scaleDownCoroutineIsRunning = false;
            _scaledUp = false;
        }

        private IEnumerator ScaleUp()
        {
            _scaleUpCoroutineIsRunning = true;

            for (int i = 0; i < 10; i++)
            {
                if (transform.localScale.x >= .6)
                    break;

                transform.localScale = new Vector3(
                    Mathf.Lerp(transform.localScale.x, transform.localScale.x + 0.02f, Mathf.SmoothStep(0f, 1f, i)),
                    Mathf.Lerp(transform.localScale.y, transform.localScale.y + 0.02f, Mathf.SmoothStep(0f, 1f, i)),
                    1
                );

                transform.position = new Vector3(
                    transform.position.x,
                    Mathf.Lerp(transform.position.y, transform.position.y + 0.06f, Mathf.SmoothStep(0f, 1f, i)),
                    transform.position.z
                );

                yield return new WaitForSeconds(0.01f);
            }

            _scaleUpCoroutineIsRunning = false;
            _scaledUp = true;
        }

        public abstract void ExecuteEffect();

        public abstract CardData GetCardData();
        
        public Vector3 PositionInCardBank
        {
            set
            {
                _positionInCardBank = value;
                transform.position = value;
            }
        }
    }
}