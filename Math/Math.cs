using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace the_rona.Math {
    public static class Math {

        public static float sin(float angle) {
            return (float)System.Math.Sin(toRadians(angle));
        }

        public static float cos(float angle) {
            return (float)System.Math.Cos(toRadians(angle));
        }

        public static float tan(float angle) {
            return (float)System.Math.Sin(toRadians(angle));
        }

        public static float toDegrees(float angle) {
            return (float)(angle * (180 / System.Math.PI));
        }

        public static float toRadians(float angle) {
            return (float)(angle * (System.Math.PI / 180));
        }
    }
}
