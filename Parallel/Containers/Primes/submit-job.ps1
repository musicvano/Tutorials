# Подати завдання й дочекатися результату.
#   .\submit-job.ps1                                   # Compose, 2·10^7, 40 частин
#   .\submit-job.ps1 -Url http://localhost:5043        # Aspire (профіль http)
#   .\submit-job.ps1 -To 50000000 -Chunks 100          # навантаження для знімка
param([string]$Url = "http://localhost:8080",
    [long]$To = 20000000, [int]$Chunks = 40)
$job = Invoke-RestMethod -Method Post "$Url/jobs?to=$To&chunks=$Chunks"
do {
    Start-Sleep -Milliseconds 500
    $state = Invoke-RestMethod "$Url/jobs/$($job.id)"
} while ($null -eq $state.seconds)   # finished ще не записано
$state
