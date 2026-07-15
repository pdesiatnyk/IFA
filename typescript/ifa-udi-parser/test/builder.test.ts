import { describe, expect, it } from 'vitest';
import { buildUdi } from '../src/builder.js';
import { parseUdi } from '../src/parser.js';
import { IfaUdiBuildError } from '../src/types.js';
import type { BuildUdiInput, EnvelopeForm } from '../src/types.js';
import { loadFixtures } from './fixtures.js';

const fixtures = loadFixtures();

describe('buildUdi', () => {
  for (const fixture of fixtures.buildFixtures) {
    const envelopeForm = fixture.envelopeForm as EnvelopeForm | undefined;
    const input = fixture.input as unknown as BuildUdiInput;

    if (!fixture.expectedValid) {
      it(`throws for ${fixture.name} (reason ${fixture.expectedReason})`, () => {
        try {
          buildUdi(input, envelopeForm);
          expect.fail('expected buildUdi to throw');
        } catch (err) {
          expect(err).toBeInstanceOf(IfaUdiBuildError);
          expect((err as IfaUdiBuildError).reason).toBe(fixture.expectedReason);
        }
      });
      continue;
    }

    it(`builds ${fixture.name} into the expected output`, () => {
      const output = buildUdi(input, envelopeForm);
      expect(output).toBe(fixture.expectedOutput);
    });

    it(`round-trips ${fixture.name} through parseUdi`, () => {
      const output = buildUdi(input, envelopeForm);
      const parsed = parseUdi(output);

      if (input.udiDi.scheme === 'PPN') {
        expect(parsed.udiDi.scheme).toBe('PPN');
        expect(parsed.udiDi.pzn?.slice(0, 7)).toBe(input.udiDi.pznBase);
      } else if (input.udiDi.scheme === 'HPC') {
        expect(parsed.udiDi.scheme).toBe('HPC');
        expect(parsed.udiDi.cin).toBe(input.udiDi.cin);
        expect(parsed.udiDi.itemReference).toBe(input.udiDi.itemReference);
        expect(parsed.udiDi.packagingLevelIndex).toBe(input.udiDi.packagingLevelIndex);
      } else if (input.udiDi.scheme === 'MASTER_UDI_DI') {
        expect(parsed.udiDi.scheme).toBe('MASTER_UDI_DI');
        expect(parsed.udiDi.cin).toBe(input.udiDi.cin);
        expect(parsed.udiDi.deviceGroupCode).toBe(input.udiDi.deviceGroupCode);
      } else {
        expect(parsed.udiDi.scheme).toBe(input.udiDi.scheme);
        expect(parsed.udiDi.nationalCode).toBe(input.udiDi.nationalCode);
      }

      if (input.udiPi) {
        expect(parsed.udiPi.lot).toBe(input.udiPi.lot);
        expect(parsed.udiPi.expiryDate).toBe(input.udiPi.expiryDate);
        expect(parsed.udiPi.manufacturingDate).toBe(input.udiPi.manufacturingDate);
        expect(parsed.udiPi.serialNumber).toBe(input.udiPi.serialNumber);
        expect(parsed.udiPi.quantity).toBe(input.udiPi.quantity);
        expect(parsed.udiPi.price).toBe(input.udiPi.price);
        expect(parsed.udiPi.url).toBe(input.udiPi.url);
        expect(parsed.udiPi.additionalGtins).toEqual(input.udiPi.additionalGtins);
      }
    });
  }

  it('defaults to interpretationLine when envelopeForm is omitted', () => {
    const output = buildUdi({ udiDi: { scheme: 'PPN', pznBase: '1234567' } });
    expect(output).toBe('(9N)111234567842');
  });

  it('rejects din16598 for non-HPC schemes with the DIN16598_HPC_ONLY reason', () => {
    expect(() => buildUdi({ udiDi: { scheme: 'PPN', pznBase: '1234567' } }, 'din16598')).toThrowError(IfaUdiBuildError);
  });
});
