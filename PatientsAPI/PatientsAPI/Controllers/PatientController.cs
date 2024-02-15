using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace PatientsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly string _connectionString = "server=host.docker.internal;port=3308;database=api_test;uid=root;pwd=1qaz@WSX3edc;";

        [HttpGet("GetPatients")]
        public async Task<IEnumerable<PatientWithId>> GetPatients([FromQuery] string? birthDate = null)
        {
            List<PatientWithId> patients = new List<PatientWithId>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sqlText = "SELECT * FROM patient";
                bool birthDateCondition = false;
                DateTime comparisonDate = DateTime.MinValue; 
                if (birthDate != null)
                {
                    string comparisonOperator = birthDate.Substring(0, 2);
                    string dateString = birthDate.Substring(2);
                    if (DateTime.TryParse(dateString, out comparisonDate))
                    {
                        sqlText += SelectQueryDateCondition(comparisonOperator, ref birthDateCondition);
                    }
                }
                using var command = new MySqlCommand(sqlText, connection);
                if (birthDateCondition)
                {
                    command.Parameters.AddWithValue("@BirthDate", comparisonDate);
                }
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    patients.Add(new PatientWithId
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
                    });
                }
            }
            return patients;
        }

        private string SelectQueryDateCondition(string comparisonOperator, ref bool birthDateCondition)
        {
            string condition;
            switch (comparisonOperator.ToLower())
            {
                case "eq":
                    condition = " WHERE DATE(BirthDate) = @BirthDate";
                    break;
                case "ne":
                    condition = " WHERE DATE(BirthDate) <> @BirthDate";
                    break;
                case "lt":
                    condition = " WHERE DATE(BirthDate) < @BirthDate";
                    break;
                case "le":
                    condition = " WHERE DATE(BirthDate) <= @BirthDate";
                    break;
                case "gt":
                    condition = " WHERE DATE(BirthDate) > @BirthDate";
                    break;
                case "ge":
                    condition = " WHERE DATE(BirthDate) >= @BirthDate";
                    break;
                default:
                    throw new ArgumentException("Не верный оператор сравнения.");
            }
            birthDateCondition = true;
            return condition;
        }

        [HttpGet("GetPatient")]
        public async Task<ActionResult<PatientWithId?>> GetPatient(Guid id)
        {
            PatientWithId patient = null;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM patient WHERE Id = @Id";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    patient = new PatientWithId
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
                command.Parameters.AddWithValue("@UseName", patient.Name?.Use);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@Active", patient.Active);
                command.Parameters.AddWithValue("@FamilyName", patient.Name?.Family);
                if (patient.Name?.Given != null)
                    command.Parameters.AddWithValue("@GivenName", string.Join(' ', patient.Name.Given));
                else
                    command.Parameters.AddWithValue("@GivenName", "");
                command.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
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
                command.Parameters.AddWithValue("@UseName", patient.Name?.Use);
                command.Parameters.AddWithValue("@FamilyName", patient.Name?.Family);
                if (patient.Name?.Given != null)
                    command.Parameters.AddWithValue("@GivenName", string.Join(' ', patient.Name.Given));
                else
                    command.Parameters.AddWithValue("@GivenName", "");
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@BirthDate", patient.BirthDate);
                command.Parameters.AddWithValue("@Active", patient.Active);
                command.Parameters.AddWithValue("@Id", id);
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

    }
}
