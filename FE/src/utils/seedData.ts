// Seed sample data for testing batch functionality

export const seedSampleBatchData = () => {
  const existingAssets = JSON.parse(localStorage.getItem('assets') || '[]');
  
  // Only seed if there's no data
  if (existingAssets.length > 0) {
    return;
  }
  
  const batchId1 = 'BATCH-1709640000000';
  const batchId2 = 'BATCH-1709640100000';
  
  const sampleAssets = [
    // Batch 1: 5 chairs
    ...Array.from({ length: 5 }, (_, i) => ({
      code: `GHE-2026-${(i + 1).toString().padStart(3, '0')}`,
      name: 'Ghế văn phòng cao cấp',
      assetGroup: 'chair',
      purchaseDate: '2026-03-01',
      originalValue: '2500000',
      accountCode: '2112',
      depreciationMethod: 'straight-line',
      depreciationPeriod: '60',
      department: 'Admin',
      recipient: i === 0 ? 'Nguyễn Văn A' : i === 1 ? 'Trần Thị B' : 'Chưa cấp phát',
      serialNumber: `GHE-SN-${Date.now()}-${i + 1}`,
      manufacturer: 'Hòa Phát',
      description: 'Ghế văn phòng lưng cao, có tựa đầu, điều chỉnh độ cao',
      material: 'Da PU, khung thép',
      dimensions: '60 x 60 x 120',
      monthlyDepreciation: 41666.67,
      status: 'active',
      createdAt: new Date('2026-03-01').toISOString(),
      batchId: batchId1,
      batchIndex: i + 1,
      batchTotal: 5,
    })),
    
    // Batch 2: 10 desks
    ...Array.from({ length: 10 }, (_, i) => ({
      code: `BAN-2026-${(i + 1).toString().padStart(3, '0')}`,
      name: 'Bàn làm việc văn phòng',
      assetGroup: 'desk',
      purchaseDate: '2026-03-02',
      originalValue: '4500000',
      accountCode: '2112',
      depreciationMethod: 'straight-line',
      depreciationPeriod: '72',
      department: 'Admin',
      recipient: i < 3 ? `Nhân viên ${i + 1}` : 'Chưa cấp phát',
      serialNumber: `BAN-SN-${Date.now()}-${i + 1}`,
      manufacturer: 'Nội thất 190',
      description: 'Bàn làm việc chữ L, có ngăn kéo',
      material: 'Gỗ công nghiệp phủ Melamine',
      dimensions: '140 x 70 x 75',
      monthlyDepreciation: 62500,
      status: 'active',
      createdAt: new Date('2026-03-02').toISOString(),
      batchId: batchId2,
      batchIndex: i + 1,
      batchTotal: 10,
    })),
  ];
  
  localStorage.setItem('assets', JSON.stringify(sampleAssets));
  console.log('✅ Seeded sample batch data:', sampleAssets.length, 'assets');
};

export const clearAllAssets = () => {
  localStorage.removeItem('assets');
  console.log('🗑️ Cleared all assets');
};
