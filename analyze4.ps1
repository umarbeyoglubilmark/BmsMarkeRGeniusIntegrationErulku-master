# Kasa bazinda analiz
$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw

$pattern = '"posDocumentId":\s*(\d+),\s*"receiptNo":\s*"([^"]*)",\s*"total":\s*([\d.-]+),\s*"documentType":\s*(\d+),\s*"documentTypeName":\s*"([^"]*)"[^}]*"posCode":\s*"(\d+)"'
$matches = [regex]::Matches($content, $pattern)

$kasalar = @{}

foreach($m in $matches) {
    $posCode = $m.Groups[6].Value
    $total = [decimal]$m.Groups[3].Value
    $docType = $m.Groups[5].Value

    if(-not $kasalar.ContainsKey($posCode)) {
        $kasalar[$posCode] = @{
            Count = 0
            Total = 0
            Sales = 0
            Returns = 0
            SalesAmount = 0
            ReturnsAmount = 0
        }
    }

    $kasalar[$posCode].Count++
    $kasalar[$posCode].Total += $total

    if($docType -eq "İade" -or $total -lt 0) {
        $kasalar[$posCode].Returns++
        $kasalar[$posCode].ReturnsAmount += [Math]::Abs($total)
    } else {
        $kasalar[$posCode].Sales++
        $kasalar[$posCode].SalesAmount += $total
    }
}

Write-Host "KASA BAZINDA ANALIZ"
Write-Host "==================="
Write-Host ""

foreach($kasa in $kasalar.Keys | Sort-Object) {
    $k = $kasalar[$kasa]
    Write-Host "Kasa $kasa :"
    Write-Host "  Toplam: $($k.Count) adet, $($k.Total.ToString('N2')) TL"
    Write-Host "  Satis: $($k.Sales) adet, $($k.SalesAmount.ToString('N2')) TL"
    Write-Host "  Iade: $($k.Returns) adet, $($k.ReturnsAmount.ToString('N2')) TL"
    Write-Host ""
}
