using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace the_rona.Utils
{
   public class Timer {
        float time_of_start = 0;
        bool started = false;

        public Timer() { }

        public void start() {
            started = true;
            time_of_start = Globals.time_elapsed;
        }

        public void pause() {
            started = false;
        }

        public void unpause() {
            started = true;
        }

        public bool didStart() {
            return started;
        }

        public float getTimeElapsed() {
            if (!started) return 0;
            return Globals.time_elapsed - time_of_start;
        }        
    }
}
