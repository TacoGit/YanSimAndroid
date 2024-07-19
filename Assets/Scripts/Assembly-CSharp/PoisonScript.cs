using UnityEngine;

public class PoisonScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	public GameObject Bottle;

	public void Start()
	{
		base.gameObject.SetActive(ClassGlobals.ChemistryGrade + ClassGlobals.ChemistryBonus >= 1);
	}

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			Yandere.Inventory.ChemicalPoison = true;
			Yandere.StudentManager.UpdateAllBentos();
			Object.Destroy(base.gameObject);
			Object.Destroy(Bottle);
		}
	}
}
