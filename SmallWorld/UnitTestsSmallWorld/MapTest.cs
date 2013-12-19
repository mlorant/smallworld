using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallWorld;
using System.Drawing;

namespace UnitTestsSmallWorld
{
    /// <summary>
    /// Description résumée pour MapTest
    /// </summary>
    [TestClass]
    public class MapTest
    {

        Map map;

        public MapTest()
        {
            map = new Map();
        }

        [TestMethod]
        public void TestFlyWeight()
        {
            int width = 10;
            List<ICase> instances = new List<ICase>();
            map.generateMap(width);
            
            for (int i = 0; i < width*width; i++)
            {
                
                ICase inst = map.getCase(new Point(i / width, i % width));

                bool isInList = false;
                for (int j = 0; j < instances.Count; j++)
                {
                    if (ReferenceEquals(inst, instances[j]))
                    {
                        isInList = true;
                    }
                }

                if (!isInList)
                {
                    instances.Add(inst);
                }
            }

            Assert.IsTrue(instances.Count == 5);

        }

        [TestMethod]
        public void TestMapSize()
        {
            map.generateMap(20);
            Assert.IsTrue(map.Size == 20);
        }
    }
}
