
Function SearchInArr($arr, $string)
{
    foreach($element in $arr)
    {
        if ($element.Name -eq $string)
        {
            return $element
        }
    }
    return $null
}

Function GroupsArrayToChangeCountNameArray($groupArr)
{
    $toRet = New-Object System.Collections.ArrayList

    foreach($row in $groupArr)
    {
        $sum = 0
        foreach($activity in $row.Group)
        {
            $sum += [Math]::Abs($activity.oldlen - $activity.newlen)
        }

    
        $obj = New-Object System.Object
        $obj | Add-Member -MemberType NoteProperty -Name "Name" -Value ($row.Name)
        $obj | Add-Member -MemberType NoteProperty -Name "Count" -Value ($row.Count)
        $obj | Add-Member -MemberType NoteProperty -Name "Change" -Value ($sum)

        $toRet.Add($obj) | Out-Null
    }
    return $toRet
}
<#
Function JoinArrays([System.Object[]]$arrs)
{
    $keys = New-Object System.Collections.Generic.HashSet[string]
    foreach($arr in $arrs)
    {
        foreach($entry in $arr)
        {
            $keys.Add($entry.Name) | Out-Null
        }
    }

    $result = New-Object System.Collections.ArrayList


    foreach($key in $keys)
    {
        $obj = New-Object System.Object
        $obj | Add-Member -MemberType NoteProperty -Name "Name" -Value $key

        foreach($arr in $arrs)
        {
            
        }

        $obj | Add-Member -MemberType NoteProperty -Name "Talk" -Value ((SearchInArr $byTalkCount $name).Count)
        $obj | Add-Member -MemberType NoteProperty -Name "Talk Len Change" -Value ((SearchInArr $byTalkCount $name).Change)
        $obj | Add-Member -MemberType NoteProperty -Name "Recent Changes" -Value ((SearchInArr $byRecentChanges $name).Count)
        $obj | Add-Member -MemberType NoteProperty -Name "Recent Changes Len Change" -Value ((SearchInArr $byRecentChanges $name).Change)
    

        $result.Add($obj) | Out-Null
    }
}
#>

$baseAddress = "https://he.wikipedia.org"
$restResult = Invoke-RestMethod ($baseAddress + "/w/api.php?action=query&format=json&list=recentchanges&rcnamespace=1&rcprop=title|sizes&rclimit=max")
$byTalkCount = $restResult.query.recentchanges | Group-Object {$_.title} | Select-Object @{Name="Name"; Expression = {$_.Name.Substring(5)}}, Count, Group

$byTalkCount = GroupsArrayToChangeCountNameArray $byTalkCount


$restResult = Invoke-RestMethod ($baseAddress + "/w/api.php?action=query&format=json&list=recentchanges&rcnamespace=0&rcprop=title%7Csizes&rclimit=max")
$byRecentChanges = $restResult.query.recentchanges | Group-Object {$_.title}

$byRecentChanges = GroupsArrayToChangeCountNameArray $byRecentChanges

$names = $byTalkCount | ForEach-Object {$_.Name}
$names += $byRecentChanges | ForEach-Object {$_.Name}
$names = $names | Select-Object -Unique | Sort-Object

$result = New-Object System.Collections.ArrayList

foreach($name in $names)
{
    $elementInByTalkCount = SearchInArr $byTalkCount $name

    $obj = New-Object System.Object
    $obj | Add-Member -MemberType NoteProperty -Name "Name" -Value $name
    $obj | Add-Member -MemberType NoteProperty -Name "Talk" -Value ($elementInByTalkCount.Count)
    $obj | Add-Member -MemberType NoteProperty -Name "Talk Len Change" -Value ($elementInByTalkCount.Change)
    $obj | Add-Member -MemberType NoteProperty -Name "Recent Changes" -Value ((SearchInArr $byRecentChanges $name).Count)
    $obj | Add-Member -MemberType NoteProperty -Name "Recent Changes Len Change" -Value ((SearchInArr $byRecentChanges $name).Change)
    

    $result.Add($obj) | Out-Null
}

$result | Out-GridView
$result | Export-Csv -Path C:\Users\guyav\Desktop\humus.csv -NoTypeInformation -Encoding UTF8