Param($URL, $PAT, $MODE)
$groupVariables = Invoke-RestMethod -Uri $URL -Method Get -Headers @{Authorization=("Bearer {0}" -f $PAT)}
$groupVariables.variables.Revision.value = $groupVariables.variables.Revision.value/1 + 1;
# switch ($MODE) 
# {
   # "MAJOR" { $groupVariables.variables.Major.value = $groupVariables.variables.Major.value/1 + 1; break; }
   # "MINOR" { $groupVariables.variables.Minor.value = $groupVariables.variables.Minor.value/1 + 1; break; }
   # "REVISION" { $groupVariables.variables.Revision.value = $groupVariables.variables.Revision.value/1 + 1; break; }
   # default { throw "Something else happened";}
# }
$json = ($groupVariables | ConvertTo-Json -Compress).ToString()
$pipeline = Invoke-RestMethod -Uri $URL -Method Put -Body $json -ContentType "application/json" -Headers @{Authorization=("Bearer {0}" -f $PAT)}