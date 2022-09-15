Param($URL, $PAT, $MODE)

# Base64-encodes the Personal Access Token (PAT) appropriately
$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f "", $PAT)))

Write-Host "The URL for this call: " $URL

# Get the variable group by id
$groupVariables = Invoke-RestMethod -Uri $URL -Method Get -Headers @{Authorization=("Bearer {0}" -f $PAT)}

Write-Host "Variables call result: " $groupVariables.variables

# Update the necessary varible values
switch ($MODE) 
{
   "MAJOR" { $groupVariables.variables.Major.value = $groupVariables.variables.Major.value/1 + 1; break; }
   "MINOR" { $groupVariables.variables.Minor.value = $groupVariables.variables.Minor.value/1 + 1; break; }
   "REVISION" { $groupVariables.variables.Revision.value = $groupVariables.variables.Revision.value/1 + 1; break; }
   default { throw "Something else happened";}
}

# Convert to Json
$json = ($groupVariables | ConvertTo-Json -Compress).ToString()
      
Write-Host "After modifying and Jsonifying the variables: " $json

# Send the updated varible group back to Azure DevOps
$pipeline = Invoke-RestMethod -Uri $URL -Method Put -Body $json -ContentType "application/json" -Headers @{Authorization=("Bearer {0}" -f $PAT)}

Write-Host "Updating variable group was successful."