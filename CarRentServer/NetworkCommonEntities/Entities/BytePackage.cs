using System.Text;

namespace NetworkCommonEntities.Entities
{

    public enum ServerPackets { 
        welcome,
        disonnectHost,
        disconnectClient,
        sendOneEntity,
        sendListOfEntities,
        userIsRegistrated,
        userVerified,
        confirmCRUD,
        confirmChangingPassword,
        confirmSaving,
        carsFiltration,
        carsForReview,
        allCarsForRent,
        allUsersForRent,
        rentedCarsByUser,
        rentsByUser,
        reviewsByUser,
        reviewsByCar,
        reviewAuthor
    }

    public enum ClientPackets { 
        welcomeReceived,
        disconnect,
        userRegistration,
        userVerification,
        getAll,
        getById,
        deleteEntity,
        changeUserPassword,
        getAllReviewsByUser,
        getAllRentsByUser,
        getCarsByFiltration,
        saveEntity,
        getCarBodies,
        getCarManufacturers,
        getCarsForReview, 
        getAllCarsForRent,
        getAllUsersForRent,
        getRentedCarsByUser,
        getAllReviewsByCar,
        getReviewAuthor
    }
    public class BytePackage : IDisposable
    {
        public int Length
        {
            get
            {
                return buffer.Count;
            }
        }

        public int UnreadLength
        {
            get
            {
                return buffer.Count - readPos;
            }
        }

        public BytePackage()
        {
            buffer = new List<byte>();
            readPos = 0;
        }

        public BytePackage(int _id)
        {
            buffer = new List<byte>();
            readPos = 0;
            Write(_id);
        }

        public BytePackage(byte[] _data)
        {
            buffer = new List<byte>();
            readPos = 0;
            SetBytes(_data);
        }

        public void SetBytes(byte[] _data)
        {
            Write(_data);
            readableBuffer = buffer.ToArray();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
        }

        public void InsertInt(int _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value));
        }

        public byte[] ToArray()
        {
            readableBuffer = buffer.ToArray();
            return readableBuffer;
        }

        public void Reset(bool _shouldReset = true)
        {
            if (_shouldReset)
            {
                buffer.Clear();
                readableBuffer = null;
                readPos = 0;
            }
            else
            {
                readPos -= 4;
            }
        }
        public void Write(byte _value)
        {
            buffer.Add(_value);
        }
        public void Write(byte[] _value)
        {
            buffer.AddRange(_value);
        }
        public void Write(short _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        public void Write(int _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        public void Write(long _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        public void Write(float _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        public void Write(bool _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        public void Write(string _value)
        {
            Write(_value.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(_value));
        }

        public byte ReadByte(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                byte _value = readableBuffer[readPos];
                if (_moveReadPos)
                    readPos += 1;
                return _value;
            }
            else
            {
                throw new PackageException("Could not read value of type 'byte'!");
            }
        }

        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                byte[] _value = buffer.GetRange(readPos, _length).ToArray();
                if (_moveReadPos)
                    readPos += _length;
                return _value;
            }
            else
            {
                throw new PackageException("Could not read value of type 'byte[]'!");
            }
        }
        public short ReadShort(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                short _value = BitConverter.ToInt16(readableBuffer, readPos);
                if (_moveReadPos)
                    readPos += 2;
                return _value;
            }
            else
            {
                throw new PackageException("Could not read value of type 'short'!");
            }
        }

        public int ReadInt(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                int _value = BitConverter.ToInt32(readableBuffer, readPos);
                if (_moveReadPos)
                    readPos += 4;
                return _value;
            }
            else
            {
                throw new PackageException("Could not read value of type 'int'!");
            }
        }
        public long ReadLong(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                long _value = BitConverter.ToInt64(readableBuffer, readPos);
                if (_moveReadPos)
                    readPos += 8;
                return _value;
            }
            else
            {
                throw new PackageException("Could not read value of type 'long'!");
            }
        }
        public float ReadFloat(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                float _value = BitConverter.ToSingle(readableBuffer, readPos);
                if (_moveReadPos)
                    readPos += 4;
                return _value;
            }
            else
            {
                throw new PackageException("Could not read value of type 'float'!");
            }
        }

        public bool ReadBool(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                bool _value = BitConverter.ToBoolean(readableBuffer, readPos);
                if (_moveReadPos)
                    readPos += 1;
                return _value;
            }
            else
            {
                throw new PackageException("Could not read value of type 'bool'!");
            }
        }
        public string ReadString(bool _moveReadPos = true)
        {
            try
            {
                int _length = ReadInt();
                string _value = Encoding.ASCII.GetString(readableBuffer, readPos, _length);
                if (_moveReadPos && _value.Length > 0)
                    readPos += _length;
                return _value;
            }
            catch
            {
                throw new PackageException("Could not read value of type 'string'!");
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool _disposing)
        {
            if (!disposed)
            {
                if (_disposing)
                {
                    buffer = null;
                    readableBuffer = null;
                    readPos = 0;
                }

                disposed = true;
            }
        }

        private List<byte> buffer;
        private byte[] readableBuffer;
        private int readPos;
    }
}
