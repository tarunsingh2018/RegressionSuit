using Framework.MarketIT.Automation_Framework.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MarketIT.Automation_Framework.Managers
{
    public class FileReaderManager
    {
        private static FileReaderManager fileReaderManager;
        private static ConfigFileReader configFileReader;

        public FileReaderManager()
        {
        }

        public static FileReaderManager GetInstance()
        {
            if(fileReaderManager == null)
                fileReaderManager = new FileReaderManager();

            return fileReaderManager;
        }

        public ConfigFileReader GetConfigReader()
        {
            if (configFileReader == null)
                configFileReader = new ConfigFileReader();

            return configFileReader;
        }
    }
}
