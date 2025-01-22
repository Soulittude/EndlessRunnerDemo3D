using UnityEngine;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
	public float horizontalCurveStr;
	public float verticalCurveStr;

	int m_CurveStrengthID;
    int m_CurveStrengthID2;

    private void OnEnable()
    {
		m_CurveStrengthID = Shader.PropertyToID("_CurveStrength");
        m_CurveStrengthID2 = Shader.PropertyToID("_VerticalCurveStrength");
    }

	void Update()
	{
		Shader.SetGlobalFloat(m_CurveStrengthID, horizontalCurveStr);
		Shader.SetGlobalFloat(m_CurveStrengthID2, verticalCurveStr);
	}
}