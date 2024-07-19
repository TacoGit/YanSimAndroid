using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	[Range(0f, 1f)]
	public float from = 1f;

	[Range(0f, 1f)]
	public float to = 1f;

	private bool mCached;

	private UIRect mRect;

	private Material mMat;

	private Light mLight;

	private SpriteRenderer mSr;

	private float mBaseIntensity = 1f;

	[Obsolete("Use 'value' instead")]
	public float alpha
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public float value
	{
		get
		{
			if (!mCached)
			{
				Cache();
			}
			if (mRect != null)
			{
				return mRect.alpha;
			}
			if (mSr != null)
			{
				return mSr.color.a;
			}
			return (!(mMat != null)) ? 1f : mMat.color.a;
		}
		set
		{
			if (!mCached)
			{
				Cache();
			}
			if (mRect != null)
			{
				mRect.alpha = value;
			}
			else if (mSr != null)
			{
				Color color = mSr.color;
				color.a = value;
				mSr.color = color;
			}
			else if (mMat != null)
			{
				Color color2 = mMat.color;
				color2.a = value;
				mMat.color = color2;
			}
			else if (mLight != null)
			{
				mLight.intensity = mBaseIntensity * value;
			}
		}
	}

	private void Cache()
	{
		mCached = true;
		mRect = GetComponent<UIRect>();
		mSr = GetComponent<SpriteRenderer>();
		if (!(mRect == null) || !(mSr == null))
		{
			return;
		}
		mLight = GetComponent<Light>();
		if (mLight == null)
		{
			Renderer component = GetComponent<Renderer>();
			if (component != null)
			{
				mMat = component.material;
			}
			if (mMat == null)
			{
				mRect = GetComponentInChildren<UIRect>();
			}
		}
		else
		{
			mBaseIntensity = mLight.intensity;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		value = Mathf.Lerp(from, to, factor);
	}

	public static TweenAlpha Begin(GameObject go, float duration, float alpha, float delay = 0f)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration, delay);
		tweenAlpha.from = tweenAlpha.value;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}

	public override void SetStartToCurrentValue()
	{
		from = value;
	}

	public override void SetEndToCurrentValue()
	{
		to = value;
	}
}
