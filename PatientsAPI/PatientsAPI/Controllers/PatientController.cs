using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace PatientsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly string _connectionString = "server=localhost;port=3306;database=api_test;uid=root;pwd=1qaz@WSX3edc;";

        [HttpGet("GetPatients")]
        public async Task<IEnumerable<PatientWithId>> GetPatients([FromQuery] string? birthDate = null)
        {
            List<PatientWithId> patients = new List<PatientWithId>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "SELECT * FROM patient";
                var (birthDateCondition, comparisonDate) = BirthDateCondition(birthDate);
                using var command = new MySqlCommand(sqlText, connection);
                if (birthDateCondition != null)
                {
                    command.Parameters.AddWithValue("@BirthDate", comparisonDate);
                }
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    patients.Add(PatientDTO(reader));
                }
            }
            return patients;
        }

        [HttpGet("GetPatient")]
        public async Task<ActionResult<PatientWithId?>> GetPatient(Guid id)
        {
            PatientWithId? patient = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM patient WHERE Id = @Id";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    patient = PatientDTO(reader);
                }
            }
            return patient;
        }

        [HttpPost("PostPatient")]
        public async Task<IActionResult> PostPatient(Patient patient)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "INSERT INTO patient (UseName, Gender, Active, FamilyName, GivenName, BirthDate) " +
                             "VALUES (@UseName, @Gender, @Active, @FamilyName, @GivenName, @BirthDate)";
                using var command = new MySqlCommand(sqlText, connection);
                SetSQLParameters(patient, command.Parameters);
                await command.ExecuteNonQueryAsync();
            }
            return NoContent();
        }

        [HttpPut("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient(Guid id, Patient patient)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "UPDATE patient SET UseName = @UseName, FamilyName = @FamilyName, " +
                             "GivenName = @GivenName, Gender = @Gender, BirthDate = @BirthDate, Active = @Active " +
                             "WHERE Id = @Id";
                using var command = new MySqlCommand(sqlText, connection);
                SetSQLParameters(patient, command.Parameters, id);
                await command.ExecuteNonQueryAsync();
            }
            return NoContent();
        }

        [HttpDelete("DeletePatient")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "DELETE FROM patient WHERE Id = @Id";
                using var command = new MySqlCommand(sqlText, connection);
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
            return NoContent();
        }

        private static void SetSQLParameters(Patient patient, MySqlParameterCollection parameters, Guid? id = null) {
            parameters.AddWithValue("@UseName", patient.Name?.Use);
            parameters.AddWithValue("@FamilyName", patient.Name?.Family);
            if (patient.Name?.Given != null)
                parameters.AddWithValue("@GivenName", string.Join(' ', patient.Name.Given));
            else
                parameters.AddWithValue("@GivenName", "");
            parameters.AddWithValue("@Gender", patient.Gender);
            parameters.AddWithValue("@BirthDate", patient.BirthDate);
            parameters.AddWithValue("@Active", patient.Active);
            if (id != null)
            {
                parameters.AddWithValue("@Id", id);
            }
        }
        
        private static PatientWithId PatientDTO(DbDataReader reader) 
        {
            return new PatientWithId
                {
                    Name = new NameWithId
                    {
                        Id = reader.GetGuid("Id"),
                        Use = reader.GetString("UseName"),
                        Family = reader.GetString("FamilyName"),
                        Given = reader.GetString("GivenName").Split(' ')
                    },
                    Gender = reader.GetString("Gender"),
                    BirthDate = reader.GetDateTime("BirthDate"),
                    Active = reader.GetBoolean("Active")
                };
        }

        private static (string?, DateTime) BirthDateCondition(string? birthDate) {
            DateTime comparisonDate = DateTime.MinValue; 
            if (birthDate != null)
            {
                string comparisonOperator = birthDate[..2];
                string dateString = birthDate[2..];
                if (DateTime.TryParse(dateString, out comparisonDate))
                {
                    return (SelectQueryDateCondition(comparisonOperator), comparisonDate);
                }
            }
            return (null, comparisonDate);
        }

        private static string SelectQueryDateCondition(string comparisonOperator)
        {
            string sqlOperator = comparisonOperator.ToLower() switch
            {
                "eq" => "=",
                "ne" => "<>",
                "lt" => "<",
                "le" => "<",
                "gt" => ">",
                "ge" => ">=",
                _ => throw new ArgumentException("Не верный оператор сравнения."),
            };
            return $" WHERE DATE(BirthDate) {sqlOperator} @BirthDate";
        }
    }
}
