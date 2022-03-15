using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using Object = UnityEngine.Object;

// 게임 실행중 로드한 리소스들을 관리하는 클래스입니다.
public sealed class ResourceManager : ManagerClassBase<ResourceManager>
{
	
	// 로드한 리소스들을 저장합니다.
	private Dictionary<string, Object> _LoadedResources = new Dictionary<string, Object>();

	public Object this[string resourceName] => _LoadedResources[resourceName];

	public override void InitializeManagerClass() 
	{
	}

	// 특정한 형식으로 리소스를 로드하여 반환합니다.
	public T LoadResource<T>(string resouceName, string resourcePath = null) where T : Object
	{
		// 만약 이미 resouceName 으로 등록되어있는 요소가 존재한다면
		if (_LoadedResources.ContainsKey(resouceName))
			return this[resouceName] as T;

		else
		{
			// 리소스를 로드합니다.
			T loadedResource = Resources.Load<T>(resourcePath);

			// 만약 리소스가 로드되었다면
			if (loadedResource)
			{
				_LoadedResources.Add(resouceName, loadedResource);
				return loadedResource as T;
			}
			// 리소스가 로드되지 않았다면
			else
			{
				// 에디터의 경우에만 로그를 띄웁니다.
#if UNITY_EDITOR
				Debug.LogError($"{resouceName} is not loaded! (path : {resourcePath})");
#endif

				return null;
			}
		}
	}

	// Json 파일을 저장합니다.
	public void SaveJson<T>(T data, string fileName, string path)
    {
        try
        {
			// data 를 직려화 합니다.
			string json = JsonUtility.ToJson(data, true);

			// 저장할 내용이 비어있다면 저장하지 않습니다.
			if(json == null)
            {
				Debug.Log("json null");
				return;
            }

			// 경로를 설정합니다.
			path = Application.dataPath + "/Resources/" + path + "/" + fileName + ".json";

			// 파일을 저장합니다.
			File.WriteAllText(path, json);

			Debug.Log(json);
        }
		catch(Exception e)
        {
			Debug.Log("The file was not found:" + e.Message);
        }

    }

	// Json 파일을 불러 옵니다.
	public T LoadJson<T>(string fileName, string path) where T : class
    {
		try
        {
			// 불러올 경로를 설정합니다.
			path = Application.dataPath + "/Resources/" + path + "/" + fileName + ".json";
			
			// 파일이 존재 하면 불러옵니다.
			if (File.Exists(path))
            {
				string json = File.ReadAllText(path);
				T t = JsonUtility.FromJson<T>(json);
				return t;
            }
        }
		catch(FileNotFoundException e)
        {
			Debug.Log("The file was not found:" + e.Message);
        }

		return null;
    }

}
