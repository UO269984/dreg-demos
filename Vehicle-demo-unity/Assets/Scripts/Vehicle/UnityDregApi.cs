using System;
using UnityEngine;

using AOT;
using System.Runtime.InteropServices;

public static class UnityDregApi {
	#if WEBGL && ! UNITY_EDITOR
		[DllImport("__Internal")]
		private static extern void downloadFileBrowser(IntPtr filename, IntPtr data);
	#else
		private static void downloadFileBrowser(IntPtr filename, IntPtr data) {}
	#endif
	
	static UnityDregApi() {
		Dreg.setPrintFunc(printFuncLog);
		
		if (Application.platform == RuntimePlatform.WebGLPlayer)
			Dreg.setSaveFileFunc(saveFileBrowser);
	}
	
	[MonoPInvokeCallback(typeof(Dreg.PrintFunc))]
	private static void printFuncLog(IntPtr toPrint) {
		Debug.Log("Dreg: " + Marshal.PtrToStringAnsi(toPrint));
	}
	
	[MonoPInvokeCallback(typeof(Dreg.SaveFileFunc))]
	private static void saveFileBrowser(IntPtr filename, IntPtr data) {
		downloadFileBrowser(filename, data);
	}

	public static Vector3_Dreg ToDregVec(this Vector3 vec) {
		return new Vector3_Dreg(vec.x, vec.z, vec.y);
	}

	public static Vector3 ToUnityVec(this Vector3_Dreg vecDreg) {
		return new Vector3(vecDreg.x, vecDreg.z, vecDreg.y);
	}
}