using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
	public int m_ObjectPoolSize = 5;
	public GameObject m_Prefab;
	public GameObject[] m_Pool;

	public NetworkHash128 assetId { get; set; }

	public delegate GameObject SpawnDelegate(Vector3 position, NetworkHash128 assetId);
	public delegate void UnSpawnDelegate(GameObject spawned);

	private void Start()
	{
		assetId = m_Prefab.GetComponent<NetworkIdentity>().assetId;
		m_Pool = new GameObject[m_ObjectPoolSize];

		for ( int i = 0; i < m_ObjectPoolSize; ++i )
		{
			m_Pool[i] = (GameObject)Instantiate(m_Prefab, Vector3.zero, Quaternion.identity);
			m_Pool[i].GetComponent<Poolable>().pool = this;
			m_Pool[i].name = "PoolObject" + i;
			m_Pool[i].SetActive(false);
		}

		ClientScene.RegisterSpawnHandler(assetId, SpawnObject, UnSpawnObject);
	}

	public GameObject GetFromPool(Vector3 position)
	{
		GameObject poolable;

		for ( int i = m_Pool.Length - 1; i > -1; i-- )
		{
			poolable = m_Pool[i];

			if ( !poolable.activeInHierarchy )
			{
				Debug.Log("Activating object " + poolable.name + " at " + position);
				poolable.transform.position = position;
				poolable.SetActive(true);

				return poolable;
			}
		}

		Debug.LogWarning("Could not grab object from pool, nothing available");
		return null;
	}

	public GameObject SpawnObject(Vector3 position, NetworkHash128 assetId)
	{
		return GetFromPool(position);
	}

	public void UnSpawnObject(GameObject _spawned)
	{
		Debug.Log("Re-pooling object " + _spawned.name);
		_spawned.SetActive(false);
	}

	public void returnToPool(GameObject _spawned, WaitForSeconds _delay = null)
	{
		StartCoroutine(returnToPoolCoroutine(_spawned, _delay));
	}

	public IEnumerator returnToPoolCoroutine(GameObject _spawned, WaitForSeconds _delay = null)
	{
		if ( _delay != null )
		{
			yield return _delay;
		}

		UnSpawnObject(_spawned);
	}
}
