using System;
using UnityEngine;

[Serializable]
public class PoseModeSaveData
{
	public Vector3 posePosition = default(Vector3);

	public Vector3 poseRotation = default(Vector3);

	public Vector3 poseScale = default(Vector3);

	public static PoseModeSaveData ReadFromGlobals()
	{
		PoseModeSaveData poseModeSaveData = new PoseModeSaveData();
		poseModeSaveData.posePosition = PoseModeGlobals.PosePosition;
		poseModeSaveData.poseRotation = PoseModeGlobals.PoseRotation;
		poseModeSaveData.poseScale = PoseModeGlobals.PoseScale;
		return poseModeSaveData;
	}

	public static void WriteToGlobals(PoseModeSaveData data)
	{
		PoseModeGlobals.PosePosition = data.posePosition;
		PoseModeGlobals.PoseRotation = data.poseRotation;
		PoseModeGlobals.PoseScale = data.poseScale;
	}
}
