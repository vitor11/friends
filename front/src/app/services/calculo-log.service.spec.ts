import { TestBed } from '@angular/core/testing';

import { CalculoLogService } from './calculo-log.service';

describe('CalculoLogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CalculoLogService = TestBed.get(CalculoLogService);
    expect(service).toBeTruthy();
  });
});
