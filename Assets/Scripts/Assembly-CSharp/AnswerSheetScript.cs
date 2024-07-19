using System;
using UnityEngine;

public class AnswerSheetScript : MonoBehaviour
{
	public SchemesScript Schemes;

	public DoorGapScript DoorGap;

	public PromptScript Prompt;

	public ClockScript Clock;

	public Mesh OriginalMesh;

	public MeshFilter MyMesh;

	public int Phase = 1;

	private void Start()
	{
		OriginalMesh = MyMesh.mesh;
		if (SchemeGlobals.GetSchemeStage(5) == 100)
		{
			Prompt.Hide();
			Prompt.enabled = false;
			base.gameObject.SetActive(false);
			return;
		}
		if (SchemeGlobals.GetSchemeStage(5) > 4)
		{
			Prompt.Hide();
			Prompt.enabled = false;
		}
		if (DateGlobals.Weekday == DayOfWeek.Friday && Clock.HourTime > 13.5f)
		{
			Prompt.Hide();
			Prompt.enabled = false;
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			if (Phase == 1)
			{
				SchemeGlobals.SetSchemeStage(5, 2);
				Schemes.UpdateInstructions();
				Prompt.Yandere.Inventory.AnswerSheet = true;
				Prompt.Hide();
				Prompt.enabled = false;
				DoorGap.Prompt.enabled = true;
				MyMesh.mesh = null;
				Phase++;
			}
			else
			{
				SchemeGlobals.SetSchemeStage(5, 5);
				Schemes.UpdateInstructions();
				Prompt.Yandere.Inventory.AnswerSheet = false;
				Prompt.Hide();
				Prompt.enabled = false;
				MyMesh.mesh = OriginalMesh;
				Phase++;
			}
		}
	}
}
