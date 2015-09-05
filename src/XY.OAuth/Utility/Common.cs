using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XY.OAuth.Utility
{
	public  class Common
	{
		#region 获取6位的随机数
		/// <summary>
		/// 获取6位的随机数
		/// </summary>
		/// <returns>6位的随机数</returns>
		public static int GetRandom()
		{
			Random random = new Random();
			return random.Next(100000, 1000000);
		} 
		#endregion
	}
}
