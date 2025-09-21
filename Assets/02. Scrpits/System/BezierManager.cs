using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class BezierManager : MonoBehaviour
{
	[Range(0.01f, 1f)]
	public float gizmoRadius = 0.3f;

	[Range(0f, 1f)]
	public float progression = 0f;

	protected bool HasNull<T>(params T[] elements) where T : class
	{
		for (int i = 0; i < elements.Length; i++)
			if (elements[i] == null) return true;
		return false;
	}

	protected Vector3 Lerp(Transform a, Transform b, float t)
	{
		return Vector3.Lerp(a.position, b.position, t);
	}

	protected void DrawGizmoSphere(float radius, Transform point)
	{
		Gizmos.DrawWireSphere(point.position, radius);
	}

	protected void DrawGizmoSphere(float radius, in Vector3 point)
	{
		Gizmos.DrawWireSphere(point, radius);
	}

	protected void DrawGizmoSpheres(float radius, params Vector3[] points)
	{
		for (int i = 0; i < points.Length; i++)
		{
			Gizmos.DrawWireSphere(points[i], radius);
		}
	}

	protected void DrawGizmoSpheres(float radius, params Transform[] points)
	{
		for (int i = 0; i < points.Length; i++)
		{
			Gizmos.DrawWireSphere(points[i].position, radius);
		}
	}

	protected void DrawGizmoLine(Transform a, Transform b)
	{
		Gizmos.DrawLine(a.position, b.position);
	}

	protected void DrawGizmoLine(in Vector3 a, in Vector3 b)
	{
		Gizmos.DrawLine(a, b);
	}

	protected void DrawGizmoLines(params Vector3[] points)
	{
		int len = points.Length;
		if (len < 2) return;

		for (int i = 0; i < len - 1; i++)
		{
			Gizmos.DrawLine(points[i], points[i + 1]);
		}
	}

	protected void DrawGizmoLines(params Transform[] points)
	{
		int len = points.Length;
		if (len < 2) return;

		for (int i = 0; i < len - 1; i++)
		{
			Gizmos.DrawLine(points[i].position, points[i + 1].position);
		}
	}

	[Space, Range(2, 100)]
	public int samplePointCount = 100;

	[Space]
	public Transform pointParent;
	[Space]
	public Transform[] points;

	private LineRenderer lineRenderer;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		CalculateCurvePoints(samplePointCount);
	}

	private void OnDrawGizmos()
	{
		if (points == null) return;
		for (int i = 0; i < points.Length; i++)
		{
			if (points[i] == null) return;
		}
		// ======================================================

		float radius = gizmoRadius;

		Gizmos.color = Color.red;
		for (int i = 0; i < points.Length; i++)
		{
			DrawGizmoSphere(radius, points[i]);
		}

		// ======================================================
		if (PointsChanged())
			CalculateCurvePoints(samplePointCount);

		Gizmos.color = Color.yellow;
		DrawCurve();
		DrawCurveLine();
		SavePrevious();
	}

	/***********************************************************************
	*                               Check Changes
	***********************************************************************/
	#region .
	private int _prevLength = 0;
	private int _prevSampleCount = 0;
	private Vector3[] _prevPositions;

	private bool PointsChanged()
	{
		if (points == null || _prevPositions == null || points.Length == 0) return false;

		if (_prevLength != points.Length)
			return true;

		if (_prevSampleCount != samplePointCount)
			return true;

		if (points.Length != pointParent.childCount)
			return true;

		for (int i = 0; i < points.Length && i < _prevPositions.Length; i++)
		{
			if (Vector3.SqrMagnitude(points[i].position - _prevPositions[i]) > 0.01f)
				return true;
		}
		return false;
	}

	private void SavePrevious()
	{
		if (_prevLength != points.Length)
		{
			_prevLength = points.Length;
			_prevPositions = new Vector3[_prevLength];
		}

		for (int i = 0; i < _prevLength; i++)
		{
			_prevPositions[i] = points[i].position;
		}

		_prevSampleCount = samplePointCount;
	}

	#endregion
	/***********************************************************************
	*                               Calculate & Draw Curve
	***********************************************************************/
	#region .
	private Vector3[] curvePoints;

	private void CalculateCurvePoints(int count)
	{
		if (points == null || points.Length < 2) return;
		if (points.Length != pointParent.childCount) RecalculatePoints();

		curvePoints = new Vector3[count + 1];
		float unit = 1.0f / count;

		ref Transform[] P = ref points;

		int n = P.Length - 1;
		int[] C = GetCombinationValues(n); // nCi
		float[] T = new float[n + 1];      // t^i
		float[] U = new float[n + 1];      // (1-t)^i

		// Iterate curvePoints : 0 ~ count(200)
		int k = 0; float t = 0f;
		for (; k < count + 1; k++, t += unit)
		{
			curvePoints[k] = Vector3.zero;

			T[0] = 1f;
			U[0] = 1f;
			T[1] = t;
			U[1] = 1f - t;

			// T[i] = t^i
			// U[i] = (1 - t)^i
			for (int i = 2; i <= n; i++)
			{
				T[i] = T[i - 1] * T[1];
				U[i] = U[i - 1] * U[1];
			}

			// Iterate Bezier Points : 0 ~ n(number of points - 1)
			for (int i = 0; i <= n; i++)
			{
				curvePoints[k] += C[i] * T[i] * U[n - i] * P[i].position;
			}
		}
	}

	private void RecalculatePoints()
	{
		points = new Transform[pointParent.childCount];
		
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = pointParent.GetChild(i);
		}
		return ;
	}

	private void DrawCurve()
	{
		if (points == null || points.Length < 2) return;
		if (curvePoints == null || curvePoints.Length < 2) return;

		float fLen = (curvePoints.Length - 1) * progression;
		int i = 0;
		for (; i < fLen; i++)
		{
			Gizmos.DrawLine(curvePoints[i], curvePoints[i + 1]);
		}
		Gizmos.DrawWireSphere(curvePoints[i], gizmoRadius * 0.8f);
	}

	private void DrawCurveLine()
	{
		if (points == null || points.Length < 2)
			return ;
		if (curvePoints == null || curvePoints.Length < 2)
			return ;
		if (lineRenderer == null)
			lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = true;
		lineRenderer.widthMultiplier = 40f;
		lineRenderer.positionCount = (int)((curvePoints.Length - 1) * progression);
		lineRenderer.SetPositions(curvePoints);
		return ;
	}

	private int[] GetCombinationValues(int n)
	{
		int[] arr = new int[n + 1];

		for (int r = 0; r <= n; r++)
		{
			arr[r] = Combination(n, r);
		}
		return arr;
	}

	private int Factorial(int n)
	{
		if (n == 0 || n == 1) return 1;
		if (n == 2) return 2;

		int result = n;
		for (int i = n - 1; i > 1; i--)
		{
			result *= i;
		}
		return result;
	}

	private int Permutation(int n, int r)
	{
		if (r == 0) return 1;
		if (r == 1) return n;

		int result = n;
		int end = n - r + 1;
		for (int i = n - 1; i >= end; i--)
		{
			result *= i;
		}
		return result;
	}

	private int Combination(int n, int r)
	{
		if (n == r) return 1;
		if (r == 0) return 1;

		// C(n, r) == C(n, n - r)
		if (n - r < r)
			r = n - r;

		return Permutation(n, r) / Factorial(r);
	}

	#endregion
}
