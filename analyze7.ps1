# Her fatura icin posDocumentId ve total listesi
$content = Get-Content "Z:\BmsMarkeRGeniusIntegration-master\kasalar.txt" -Raw

$pattern = '"posDocumentId":\s*(\d+),\s*"receiptNo":\s*"([^"]*)",\s*"total":\s*([\d.-]+)'
$matches = [regex]::Matches($content, $pattern)

Write-Host "PosDocumentId,ReceiptNo,Total"
foreach($m in $matches) {
    $docId = $m.Groups[1].Value
    $receiptNo = $m.Groups[2].Value
    $total = $m.Groups[3].Value
    Write-Host "$docId,$receiptNo,$total"
}
