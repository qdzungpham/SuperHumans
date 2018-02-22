package md5edf497c5140c9689b890e3bd795a7882;


public class LogInActivity
	extends md5edf497c5140c9689b890e3bd795a7882.BaseActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("SuperHumans.Droid.LogInActivity, SuperHumans.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LogInActivity.class, __md_methods);
	}


	public LogInActivity ()
	{
		super ();
		if (getClass () == LogInActivity.class)
			mono.android.TypeManager.Activate ("SuperHumans.Droid.LogInActivity, SuperHumans.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
