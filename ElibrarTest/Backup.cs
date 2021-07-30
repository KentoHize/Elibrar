using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Characters.Heroes;

namespace ElibrarTest
{
    [TestClass]
    public class Backup
    {
        [TestMethod]
        public void BackupProject()
        {
            Tina.SaveProject("WinForm", "Elibrar");
        }
    }
}
