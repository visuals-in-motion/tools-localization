using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
	[Serializable]
	public struct SheetsInfo
	{
		 public int? id;
		 public string name;
		public SheetsInfo(int? id,string name)
		{
			this.id = id;
			this.name = name;
		}
	}
}
