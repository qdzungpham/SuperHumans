using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SuperHumans.Droid.CustomUIs
{
    public class CheckableLinearLayout : LinearLayout, ICheckable
    {
        static readonly int[] CHECKED_STATE_SET = { Android.Resource.Attribute.StateChecked };

        bool mChecked = false;

        public CheckableLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public bool Checked
        {
            get
            {
                return mChecked;
            }
            set
            {
                if (value != mChecked)
                {
                    mChecked = value;
                    RefreshDrawableState();
                }
            }
        }

        public void Toggle()
        {
            Checked = !mChecked;
        }

        protected override int[] OnCreateDrawableState(int extraSpace)
        {
            int[] drawableState = base.OnCreateDrawableState(extraSpace + 1);

            if (Checked)
                MergeDrawableStates(drawableState, CHECKED_STATE_SET);

            return drawableState;
        }
    }
}