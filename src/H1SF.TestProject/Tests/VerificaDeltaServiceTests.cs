using H1SF.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;

namespace H1SF.TestProject.Tests
{
    [TestClass]
    public class VerificaDeltaServiceTests
    {
        /// <summary>
        /// Implementação simples de IDbConnection/IDbCommand/ IDataReader em memória
        /// apenas para suportar o cenário dos testes, sem uso de mocks.
        /// </summary>
        private class InMemoryDbConnection : IDbConnection
        {
            private readonly Queue<IDictionary<string, object?>> _rows;
            private ConnectionState _state = ConnectionState.Closed;

            public InMemoryDbConnection(IEnumerable<IDictionary<string, object?>> rows)
            {
                _rows = new Queue<IDictionary<string, object?>>(rows);
            }

            public string ConnectionString { get; set; } = string.Empty;
            public int ConnectionTimeout => 0;
            public string Database => "InMemory";
            public ConnectionState State => _state;

            public IDbTransaction BeginTransaction() => throw new NotSupportedException();
            public IDbTransaction BeginTransaction(IsolationLevel il) => throw new NotSupportedException();
            public void ChangeDatabase(string databaseName) => throw new NotSupportedException();
            public void Close() => _state = ConnectionState.Closed;
            public IDbCommand CreateCommand() => new InMemoryDbCommand(_rows, this);
            public void Open() => _state = ConnectionState.Open;
            public void Dispose() => Close();
        }

        private class InMemoryDbCommand : IDbCommand
        {
            private readonly Queue<IDictionary<string, object?>> _rows;

            public InMemoryDbCommand(Queue<IDictionary<string, object?>> rows, IDbConnection connection)
            {
                _rows = rows;
                Connection = connection;
                Parameters = new InMemoryParameterCollection();
            }

            public string CommandText { get; set; } = string.Empty;
            public int CommandTimeout { get; set; }
            public CommandType CommandType { get; set; } = CommandType.Text;
            public IDbConnection Connection { get; set; }
            public IDataParameterCollection Parameters { get; }
            public IDbTransaction? Transaction { get; set; }
            public UpdateRowSource UpdatedRowSource { get; set; }

            public void Cancel() { }
            public IDbDataParameter CreateParameter() => new InMemoryParameter();
            public void Dispose() { }
            public int ExecuteNonQuery() => 0;
            public IDataReader ExecuteReader() => new InMemoryDataReader(_rows);
            public IDataReader ExecuteReader(CommandBehavior behavior) => new InMemoryDataReader(_rows);
            public object? ExecuteScalar() => null;
            public void Prepare() { }
        }

        private class InMemoryParameter : IDbDataParameter
        {
            public DbType DbType { get; set; }
            public ParameterDirection Direction { get; set; } = ParameterDirection.Input;
            public bool IsNullable => true;
            public string ParameterName { get; set; } = string.Empty;
            public string SourceColumn { get; set; } = string.Empty;
            public DataRowVersion SourceVersion { get; set; } = DataRowVersion.Current;
            public object? Value { get; set; }
            public byte Precision { get; set; }
            public byte Scale { get; set; }
            public int Size { get; set; }
        }

        private class InMemoryParameterCollection : List<IDataParameter>, IDataParameterCollection
        {
            public object? this[string parameterName]
            {
                get => this.Find(p => p.ParameterName == parameterName);
                set
                {
                    if (value is IDataParameter param)
                    {
                        Add(param);
                    }
                }
            }

            public bool Contains(string parameterName) =>
                this.Exists(p => p.ParameterName == parameterName);

            public int IndexOf(string parameterName) =>
                this.FindIndex(p => p.ParameterName == parameterName);

            public void RemoveAt(string parameterName)
            {
                var index = IndexOf(parameterName);
                if (index >= 0)
                {
                    RemoveAt(index);
                }
            }
        }

        private class InMemoryDataReader : IDataReader
        {
            private readonly Queue<IDictionary<string, object?>> _rows;
            private IDictionary<string, object?>? _current;

            public InMemoryDataReader(Queue<IDictionary<string, object?>> rows)
            {
                _rows = rows;
            }

            public object this[int i] => throw new NotSupportedException();
            public object this[string name] => _current != null && _current.TryGetValue(name, out var value)
                ? value ?? DBNull.Value
                : DBNull.Value;

            public int Depth => 0;
            public bool IsClosed => false;
            public int RecordsAffected => 0;
            public int FieldCount => _current?.Count ?? 0;

            public void Close() { }
            public void Dispose() { }

            public bool GetBoolean(int i) => throw new NotSupportedException();
            public byte GetByte(int i) => throw new NotSupportedException();
            public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length) => throw new NotSupportedException();
            public char GetChar(int i) => throw new NotSupportedException();
            public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) => throw new NotSupportedException();
            public IDataReader GetData(int i) => throw new NotSupportedException();
            public string GetDataTypeName(int i) => throw new NotSupportedException();
            public DateTime GetDateTime(int i) => throw new NotSupportedException();
            public decimal GetDecimal(int i) => throw new NotSupportedException();
            public double GetDouble(int i) => throw new NotSupportedException();
            public Type GetFieldType(int i) => throw new NotSupportedException();
            public float GetFloat(int i) => throw new NotSupportedException();
            public Guid GetGuid(int i) => throw new NotSupportedException();
            public short GetInt16(int i) => throw new NotSupportedException();
            public int GetInt32(int i) => throw new NotSupportedException();
            public long GetInt64(int i) => throw new NotSupportedException();
            public string GetName(int i) => throw new NotSupportedException();
            public int GetOrdinal(string name) => throw new NotSupportedException();
            public DataTable GetSchemaTable() => throw new NotSupportedException();
            public string GetString(int i) => throw new NotSupportedException();
            public object GetValue(int i) => throw new NotSupportedException();
            public int GetValues(object[] values) => throw new NotSupportedException();
            public bool IsDBNull(int i) => throw new NotSupportedException();

            public bool NextResult() => false;

            public bool Read()
            {
                if (_rows.Count == 0)
                    return false;

                _current = _rows.Dequeue();
                return true;
            }
        }

        [TestMethod]
        public void VerificaDelta_SemLinhasRetornadas_DeveRetornarCodigoPadrao5()
        {
            // Arrange: conexão sem linhas
            var connection = new InMemoryDbConnection(Array.Empty<IDictionary<string, object?>>());
            var service = new VerificaDeltaService(connection);

            // Act
            var resultado = service.VerificaDelta("MERC", DateTime.Now, "USER", "PROC", "NF");

            // Assert
            Assert.AreEqual("5", resultado);
        }

        [TestMethod]
        public void VerificaDelta_ComLinhaCdTPgmDelt_DeveRetornar4()
        {
            // Arrange: uma linha com CD_T_PGM = 'DELT'
            var rows = new[]
            {
                new Dictionary<string, object?>
                {
                    ["CD_T_PGM"] = "DELT"
                }
            };

            var connection = new InMemoryDbConnection(rows);
            var service = new VerificaDeltaService(connection);

            // Act
            var resultado = service.VerificaDelta("MERC", DateTime.Now, "USER", "PROC", "NF");

            // Assert
            Assert.AreEqual("4", resultado);
        }

        [TestMethod]
        public void VerificaDelta_ComLinhaCdTPgmNaoDelt_DeveRetornar1()
        {
            // Arrange: uma linha com CD_T_PGM diferente de 'DELT'
            var rows = new[]
            {
                new Dictionary<string, object?>
                {
                    ["CD_T_PGM"] = "OUTR"
                }
            };

            var connection = new InMemoryDbConnection(rows);
            var service = new VerificaDeltaService(connection);

            // Act
            var resultado = service.VerificaDelta("MERC", DateTime.Now, "USER", "PROC", "NF");

            // Assert
            Assert.AreEqual("1", resultado);
        }
    }
}

