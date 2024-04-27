    using Unity.Netcode;

    public enum TentTaskStatus {
        Wait, // waiting on previous tasks to complete
        Start,
        PolesDone,
        TarpsDone,
        Done // after nails are done
    }
    
    public enum Side {
        left,
        right
    }

    public enum Count {
        one,
        two,
        three,
        four
    }

    public struct TwoBools : INetworkSerializable
    {
        public bool left;
        public bool right;

        public TwoBools(bool l, bool r) {
            left = l;
            right = r;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref left);
            serializer.SerializeValue(ref right);
        }
    }

    public struct FourBools : INetworkSerializable
    {
        public bool one;
        public bool two;
        public bool three;
        public bool four;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref one);
            serializer.SerializeValue(ref two);
            serializer.SerializeValue(ref three);
            serializer.SerializeValue(ref three);
        }
    }