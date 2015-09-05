using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XY.OAuth.Model
{
	/// <summary>
	/// QQ信息对象
	/// </summary>
    public class QQUserInfo
    {
		/// <summary>
		/// 用户在QQ空间的昵称
		/// </summary>
        public string NickName { get; set; }

		/// <summary>
		/// 大小为30×30像素的QQ空间头像URL
		/// </summary>
        public string FigureURL { get; set; }

		/// <summary>
		/// 大小为50×50像素的QQ空间头像URL
		/// </summary>
        public string FigureURL_1 { get; set; }

		/// <summary>
		/// 大小为100×100像素的QQ空间头像URL
		/// </summary>
        public string FigureURL_2 { get; set; }

		/// <summary>
		/// 大小为40×40像素的QQ头像URL
		/// </summary>
        public string FigureURL_QQ_1 { get; set; }
        
		/// <summary>
		/// 大小为100×100像素的QQ头像URL。需要注意，不是所有的用户都拥有QQ的100x100的头像，但40x40像素则是一定会有
		/// </summary>
		public string FigureURL_QQ_2 { get; set; }

		/// <summary>
		/// 性别。 如果获取不到则默认返回"男"
		/// </summary>
        public string Gender { get; set; }

		/// <summary>
		/// 标识用户是否为黄钻用户（0：不是；1：是）
		/// </summary>
        public string Is_Yellow_VIP { get; set; }

		/// <summary>
		/// 黄钻等级
		/// </summary>
        public string Yellow_VIP_Level { get; set; }

		/// <summary>
		/// 标识是否为年费黄钻用户（0：不是； 1：是）
		/// </summary>
        public string Is_Yellow_Year_VIP { get; set; }
    }
}