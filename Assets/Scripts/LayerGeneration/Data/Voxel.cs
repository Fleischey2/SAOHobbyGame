using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace terrain {
    public struct Voxel
    {
        //Convert to single float and access via accessors...
        public byte ID;

        public bool isSolid
        {
            get
            {
                return ID != 0;
            }
        }
    }
}