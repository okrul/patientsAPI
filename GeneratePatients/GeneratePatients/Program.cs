using GeneratePatients;

var generator = new Generator();
await generator.GenerateAndSendPatientsAsync(100);