/*
* 权限校验接口
* 张国伟
* 2016-11-17
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstFrame.UI
{
    interface IPermission
    {
        bool CheckPermission();
    }
}
