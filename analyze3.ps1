# kasalar.txt detayli analiz
$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw

# Her bir satis kaydini bul
$pattern = '"posDocumentId":\s*(\d+),\s*"receiptNo":\s*"([^"]*)",\s*"total":\s*([\d.-]+),\s*"documentType":\s*(\d+)'
$matches = [regex]::Matches($content, $pattern)

Write-Host "Toplam Kayit: $($matches.Count)"
Write-Host ""

$totalSales = 0
$totalReturns = 0
$salesCount = 0
$returnCount = 0

foreach($m in $matches) {
    $docId = $m.Groups[1].Value
    $receiptNo = $m.Groups[2].Value
    $total = [decimal]$m.Groups[3].Value
    $docType = [int]$m.Groups[4].Value

    if($docType -eq 3 -or $total -lt 0) {
        $totalReturns += [Math]::Abs($total)
        $returnCount++
    } else {
        $totalSales += $total
        $salesCount++
    }
}

Write-Host "Satis Kayitlari: $salesCount adet, $($totalSales.ToString('N2')) TL"
Write-Host "Iade Kayitlari: $returnCount adet, $($totalReturns.ToString('N2')) TL"
Write-Host ""
Write-Host "Net Toplam: $(($totalSales - $totalReturns).ToString('N2')) TL"

# En buyuk 10 satis
Write-Host ""
Write-Host "En Buyuk 10 Satis:"
$allRecords = @()
foreach($m in $matches) {
    $allRecords += [PSCustomObject]@{
        DocId = $m.Groups[1].Value
        ReceiptNo = $m.Groups[2].Value
        Total = [decimal]$m.Groups[3].Value
        DocType = [int]$m.Groups[4].Value
    }
}
$allRecords | Sort-Object Total -Descending | Select-Object -First 10 | Format-Table
