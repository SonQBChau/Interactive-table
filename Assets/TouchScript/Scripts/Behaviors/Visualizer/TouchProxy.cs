/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace TouchScript.Behaviors.Visualizer
{
    /// <summary>
    /// Visual cursor implementation used by TouchScript.
    /// </summary>
    [HelpURL("http://touchscript.github.io/docs/html/T_TouchScript_Behaviors_TouchProxy.htm")]
    public class TouchProxy : TouchProxyBase
    {
        /// <summary>
        /// The link to UI.Text component.
        /// </summary>
        public Text Text;
		public GameObject obj;
		private string objName;
		private string puckName = "TUIO, Object";

		//public InterfaceAnimManager myFace;

        private StringBuilder stringBuilder = new StringBuilder(64);

        #region Protected methods

        /// <inheritdoc />
        protected override void updateOnce(TouchPoint touch)
        {
			
            base.updateOnce(touch);

            stringBuilder.Length = 0;
            stringBuilder.Append("Touch id: ");
            stringBuilder.Append(touch.Id);
            gameObject.name = stringBuilder.ToString();
			objName = touch.Tags.ToString ();
            if (Text == null) return;
            if (!ShowTouchId && !ShowTags)
            {
                Text.text = "";
                return;
            }

            stringBuilder.Length = 0;
            if (ShowTouchId)
            {
                stringBuilder.Append("Id: ");
                stringBuilder.Append(touch.Id);
            }
            if (ShowTags)
            {
                if (stringBuilder.Length > 0) stringBuilder.Append("\n");
                stringBuilder.Append("Tags: ");
                stringBuilder.Append(touch.Tags.ToString());
            }
            Text.text = stringBuilder.ToString();
			if (touch.Position.y < Screen.height/2){
				oldPosition = "Bottom";
			}
			else if (touch.Position.y > Screen.height/2){
				oldPosition = "Top";
				Vector3 currentRot;
				currentRot = gameObject.transform.eulerAngles;
				currentRot.z = (currentRot.z + (180)); 
				gameObject.transform.eulerAngles = currentRot;
			}
			if (objName == puckName) {
				var props = touch.Properties;
				if (props ["ObjectId"].ToString() == "0") {
					obj.SetActive (true);
					obj.transform.localPosition = new Vector3 (100000F, -100000F, 4F);
					obj.transform.SetParent (rect.transform, false);
					obj.transform.localScale = new Vector3 (1F, 1F, 1F);
					InterfaceAnimManager childInterface = obj.GetComponent<InterfaceAnimManager> ();
					childInterface.startAppear ();
					hideOnStart ();
				}

			} 
			else {
				this.tag = "touch";
			}

        }
	

        #endregion
    }

    /// <summary>
    /// Base class for <see cref="TouchVisualizer"/> cursors.
    /// </summary>
    public class TouchProxyBase : MonoBehaviour
    {
        #region Public properties

        /// <summary>
        /// Gets or sets cursor size.
        /// </summary>
        /// <value> Cursor size in pixels. </value>
        public int Size
        {
            get { return size; }
            set
            {
                size = value;
                rect.sizeDelta = Vector2.one * size;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether touch id text should be displayed on screen.
        /// </summary>
        /// <value> <c>true</c> if touch id text should be displayed on screen; otherwise, <c>false</c>. </value>
        public bool ShowTouchId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether touch tags text should be displayed on screen.
        /// </summary>
        /// <value> <c>true</c> if touch tags text should be displayed on screen; otherwise, <c>false</c>. </value>
        public bool ShowTags { get; set; }

        #endregion

        #region Private variables

        /// <summary>
        /// Cached RectTransform.
        /// </summary>
        protected RectTransform rect;
		public string property = "Angle";

        /// <summary>
        /// Cursor size.
        /// </summary>
        protected int size = 1;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes (resets) the cursor.
        /// </summary>
        /// <param name="parent"> Parent container. </param>
        /// <param name="touch"> Touch this cursor represents. </param>
        public void Init(RectTransform parent, TouchPoint touch)
        {
            show();
            rect.SetParent(parent);
            rect.SetAsLastSibling();
            updateOnce(touch);
            update(touch);
        }

        /// <summary>
        /// Updates the touch. This method is called when the touch is moved.
        /// </summary>
        /// <param name="touch"> Touch this cursor represents. </param>
        public void UpdateTouch(TouchPoint touch)
        {
            update(touch);
        }

        /// <summary>
        /// Hides this instance.
        /// </summary>
        public void Hide()
        {
            hide();
        }
		protected virtual void hideHUD()
		{
			gameObject.SetActive (false);
			gameObject.name = "inactive touch";
		}
        #endregion

        #region Unity methods

        private void Awake()
        {
            rect = transform as RectTransform;
            if (rect == null)
            {
                Debug.LogError("TouchProxy must be on an UI element!");
                enabled = false;
                return;
            }
            rect.anchorMin = rect.anchorMax = Vector2.zero;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Hides (clears) this instance.
        /// </summary>
        protected virtual void hide()
        {
			if (gameObject.tag == "touch") {
				gameObject.SetActive (false);
				gameObject.name = "inactive touch";
			} else {
				GameObject objChild = gameObject.transform.FindChild ("Puck_0").gameObject;
				InterfaceAnimManager childInterface = objChild.GetComponent<InterfaceAnimManager>();
				childInterface.startDisappear ();
				Invoke("hideHUD",1.7f);
			}
        }


        /// <summary>
        /// Shows this instance.
        /// </summary>
        protected virtual void show()
        {
            gameObject.SetActive(true);
        }
		public void  hideOnStart()
		{
			GameObject objChild = gameObject.transform.FindChild ("Puck_0").gameObject;
			GameObject qualificationGroup = objChild.transform.FindChild ("qualificationGroup").gameObject;
			qualificationGroup.transform.localScale = new Vector3(0, 0, 0);	
			GameObject movieGroup = objChild.transform.FindChild ("movieGroup").gameObject;
			movieGroup.transform.localScale = new Vector3(0, 0, 0);	
		}

        /// <summary>
        /// This method is called once when the cursor is initialized.
        /// </summary>
        /// <param name="touch"> The touch. </param>
        protected virtual void updateOnce(TouchPoint touch) {}

        /// <summary>
        /// This method is called every time when the touch changes.
        /// </summary>
        /// <param name="touch"> The touch. </param>
        public virtual void update(TouchPoint touch)
        {
			if (touch.Tags.ToString () == "TUIO, Object") {
				updateDisplayOrientation (touch);
				updateCircularMenu (touch);
			} else {
				rect.anchoredPosition = touch.Position;
			}
        }
		public void updateDisplayOrientation(TouchPoint touch){
			if (touch.Position.y < Screen.height/2){
				currentPosition = "Bottom";
			}
			else if (touch.Position.y > Screen.height/2){
				currentPosition = "Top";
			}
			if (currentPosition == "Top" && oldPosition == "Bottom") {
				rotateObject ();
				oldPosition = currentPosition;
			} else if (currentPosition == "Bottom" && oldPosition == "Top") {
				rotateObject ();
				oldPosition = currentPosition;
			}
		}
		public void updateCircularMenu(TouchPoint touch){
			rect.anchoredPosition = touch.Position;
			var props = touch.Properties;
			GameObject objChild = gameObject.transform.FindChild ("Puck_0").transform.FindChild("circleGroup").gameObject;
			GameObject circularMenu = objChild.transform.FindChild ("circularMenu").gameObject;
			CircularMenu circularMenuScript = circularMenu.GetComponent<CircularMenu> ();
			float curPos = float.Parse(props[property].ToString()) * Mathf.Rad2Deg;
			List<Button> buttons = circularMenuScript.buttons;
			int CurMenuItem = circularMenuScript.CurMenuItem;
			int menuItems = circularMenuScript.buttons.Count;
			CurMenuItem = (int)(curPos / (360 / menuItems));
			if (CurMenuItem != OldMenuItem) {
				Image oldImage =   buttons [OldMenuItem].transform.FindChild ("buttonImage").gameObject.GetComponent<Image>();
				Color normalColor = buttons [CurMenuItem].colors.normalColor;
				normalColor.a = 1;
				oldImage.color = normalColor;
				buttons [OldMenuItem].interactable = false;
				OldMenuItem = CurMenuItem;
				Image currImage =   buttons [CurMenuItem].transform.FindChild ("buttonImage").gameObject.GetComponent<Image>();
				Color highlightColor = buttons [CurMenuItem].colors.highlightedColor;
				currImage.color = highlightColor;
				buttons [CurMenuItem].interactable = true;
			}
		}


		private int OldMenuItem = 0;
		public string currentPosition = "Empty";
		public string oldPosition = "Empty";
		public int rotationDirection = -1;
		public int rotationStep = 10;  
		private Vector3 currentRotation, targetRotation;

		private void rotateObject()
		{
			currentRotation = gameObject.transform.eulerAngles;
			targetRotation.z = (currentRotation.z + (180 * rotationDirection));
			StartCoroutine (objectRotationAnimation());
		}
		IEnumerator objectRotationAnimation()
		{
			currentRotation.z += (rotationStep * rotationDirection);
			gameObject.transform.eulerAngles = currentRotation;
			yield return new WaitForSeconds (0);
			if (((int)currentRotation.z >
				(int)targetRotation.z && rotationDirection < 0)  || 
				((int)currentRotation.z <  (int)targetRotation.z && rotationDirection > 0)) 
			{
				StartCoroutine (objectRotationAnimation());
			}
			else
			{
				gameObject.transform.eulerAngles = targetRotation;
			}

		}

        #endregion
    }

}