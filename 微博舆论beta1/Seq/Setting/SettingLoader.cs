using System;
using System.Collections.Generic;
using System.Text;

namespace 微博舆论.Setting
{
    class SettingLoader
    {
        private void Load(string fileName)
        {
            PanGuSettings.Load(fileName);
        }

        public SettingLoader(string fileName)
        {
            Load(fileName);
        }

        public SettingLoader()
        {
            string fileName = 微博舆论.Framework.Path.GetAssemblyPath() + "PanGu.xml";
            Load(fileName);
        }
    }
}
