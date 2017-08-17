using System;

namespace TestWithNowin.Logic {

    public class SampleManager : IDisposable {
        
        public SampleManager() {
        }

        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            Console.WriteLine("Disposing true!");
        }
    }
}
