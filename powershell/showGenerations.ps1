
$param1=$args[0]
For($i = 0; $i -lt 1024; $i++)
{
    clear

    $response=Invoke-RestMethod https://localhost:44340/api/Board/$param1/generation/$i
    Write-Host "Board Name: $($response.result.boardName)"    
    Write-Host "Generation: $i of 1024 `n"    
    $response.result.states
    Write-Host "`nPress Ctrl+C to abort"
    Start-Sleep -Seconds .6    
}

