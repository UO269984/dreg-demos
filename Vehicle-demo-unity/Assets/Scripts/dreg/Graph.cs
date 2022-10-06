using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Graph {
	internal IntPtr graphPtr;
	
	public Graph() {
		this.graphPtr = Dreg.createGraph();
	}
	
	internal Graph(IntPtr graphPtr) {
		this.graphPtr = graphPtr;
	}
	
	public static void SetGraphSaveInitData(bool saveInitData) {
		Dreg.setGraphSaveInitData((char) (saveInitData ? 1 : 0));
	}
	
	public static void SetDefaultBezierSamples(UIntPtr samplesPerSegment) {
		Dreg.setDefaultBezierSamples(samplesPerSegment);
	}
	
	internal static Graph FromPtr(IntPtr graphPtr) {
		return graphPtr == IntPtr.Zero ? null : new Graph(graphPtr);
	}
	
	public void Delete() {
		Dreg.deleteGraph(this.graphPtr);
	}
	
	public Graph Clone() {
		return new Graph(Dreg.cloneGraph(this.graphPtr));
	}
	
	public bool LoadLinear(Vector2[] refs) {
		IntPtr arrayPtr = Vector2ArrayToPtr(refs);
		bool ret = Dreg.loadLinearGraph(this.graphPtr, arrayPtr, (UIntPtr) refs.Length) != 0;
		Marshal.FreeHGlobal(arrayPtr);
		
		return ret;
	}
	
	public bool LoadBezier(Vector2[] refs, int samplesPerSegment) {
		IntPtr arrayPtr = Vector2ArrayToPtr(refs);
		bool ret = Dreg.loadBezierGraph(this.graphPtr, arrayPtr, (UIntPtr) refs.Length, (UIntPtr) samplesPerSegment) != 0;
		Marshal.FreeHGlobal(arrayPtr);
		
		return ret;
	}
	
	private IntPtr Vector2ArrayToPtr(Vector2[] array) {
		IntPtr arrayPtr = Marshal.AllocHGlobal(array.Length * Size.Vector2_Dreg);
		long longPtr = arrayPtr.ToInt64();
		
		foreach (Vector2 vector2 in array) {
			Marshal.StructureToPtr(new Vector2_Dreg(vector2), new IntPtr(longPtr), false);
			longPtr += Size.Vector2_Dreg;
		}
		return arrayPtr;
	}
	
	public Vector2[] GetPoints() {
		IntPtr pointsCountPtr = Marshal.AllocHGlobal(Size.Size_t);
		IntPtr pointsPtr = Dreg.getGraphPoints(this.graphPtr, pointsCountPtr);
		
		Vector2[] points = new Vector2[Marshal.ReadIntPtr(pointsCountPtr).ToInt64()];
		Marshal.FreeHGlobal(pointsCountPtr);
		
		Vector2_Dreg auxVector2 = new Vector2_Dreg();
		long curPointPtr = pointsPtr.ToInt64();
		
		for (int i = 0; i < points.Length; i++) {
			Marshal.PtrToStructure(new IntPtr(curPointPtr), auxVector2);
			points[i] = auxVector2.ToVector2();
			
			curPointPtr += Size.Vector2_Dreg;
		}
		return points;
	}
	
	public float GetY(float x) {
		return Dreg.getGraphY(this.graphPtr, x);
	}
}