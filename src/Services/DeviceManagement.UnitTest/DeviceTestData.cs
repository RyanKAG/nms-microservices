using System;
using System.Collections;
using System.Collections.Generic;

namespace DeviceManagement.UnitTest
{
    public class DeviceTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            throw new NotImplementedException();
            // yield return new[] { };
        }

        IEnumerator IEnumerable.GetEnumerator() =>  GetEnumerator();
        
    }
}