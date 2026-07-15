<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import type { BuildUdiInput, BuildUdiPiInput, EnvelopeForm, UdiScheme } from 'ifa-udi-parser';
import ResultSummary from './ResultSummary.vue';
import ComparisonTable from './ComparisonTable.vue';
import { buildWithTs } from '../services/tsBuilder.js';
import { buildWithCSharp } from '../services/csharpApi.js';
import type { BuildOutcome } from '../types.js';

const emit = defineEmits<{ 'use-in-compare': [barcode: string] }>();

const scheme = ref<UdiScheme>('PPN');

// PPN
const pznBase = ref('1234567');
// HPC + Master UDI-DI
const cin = ref('12345');
// HPC
const itemReference = ref('MED777');
const packagingLevelIndex = ref(0);
// Master UDI-DI
const deviceGroupCode = ref('MAX19');
// AIC / AIM
const nationalCode = ref('032220128');

// UDI-PI (all optional)
const lot = ref('');
const expiryDate = ref('');
const manufacturingDate = ref('');
const serialNumber = ref('');
const quantity = ref<number | null>(null);
const price = ref('');
const url = ref('');
const additionalGtinsText = ref('');

const envelopeForm = ref<EnvelopeForm>('interpretationLine');

watch(scheme, (value) => {
  if (value !== 'HPC' && envelopeForm.value === 'din16598') {
    envelopeForm.value = 'interpretationLine';
  }
});

function buildDiInput(): BuildUdiInput['udiDi'] {
  switch (scheme.value) {
    case 'PPN':
      return { scheme: 'PPN', pznBase: pznBase.value };
    case 'HPC':
      return { scheme: 'HPC', cin: cin.value, itemReference: itemReference.value, packagingLevelIndex: packagingLevelIndex.value };
    case 'MASTER_UDI_DI':
      return { scheme: 'MASTER_UDI_DI', cin: cin.value, deviceGroupCode: deviceGroupCode.value };
    case 'AIC':
      return { scheme: 'AIC', nationalCode: nationalCode.value };
    case 'AIM':
      return { scheme: 'AIM', nationalCode: nationalCode.value };
  }
}

const buildInput = computed<BuildUdiInput>(() => {
  const udiDi = buildDiInput();

  const udiPi: BuildUdiPiInput = {};
  if (lot.value) udiPi.lot = lot.value;
  if (expiryDate.value) udiPi.expiryDate = expiryDate.value;
  if (manufacturingDate.value) udiPi.manufacturingDate = manufacturingDate.value;
  if (serialNumber.value) udiPi.serialNumber = serialNumber.value;
  if (quantity.value !== null) udiPi.quantity = quantity.value;
  if (price.value) udiPi.price = price.value;
  if (url.value) udiPi.url = url.value;
  const gtins = additionalGtinsText.value
    .split('\n')
    .map((s) => s.trim())
    .filter(Boolean);
  if (gtins.length) udiPi.additionalGtins = gtins;

  return { udiDi, udiPi: Object.keys(udiPi).length ? udiPi : undefined };
});

const tsOutcome = ref<BuildOutcome>({ loading: true });
const csharpOutcome = ref<BuildOutcome>({ loading: true });
const hasGenerated = ref(false);

async function generate() {
  hasGenerated.value = true;
  tsOutcome.value = buildWithTs(buildInput.value, envelopeForm.value);
  csharpOutcome.value = { loading: true };
  try {
    csharpOutcome.value = await buildWithCSharp(buildInput.value, envelopeForm.value);
  } catch (err) {
    csharpOutcome.value = { success: false, error: { message: String(err), field: '', reason: 'NETWORK_ERROR' } };
  }
}

function useInCompare() {
  if ('success' in tsOutcome.value && tsOutcome.value.success) {
    emit('use-in-compare', tsOutcome.value.barcode);
  } else if ('success' in csharpOutcome.value && csharpOutcome.value.success) {
    emit('use-in-compare', csharpOutcome.value.barcode);
  }
}
</script>

<template>
  <div>
    <fieldset>
      <legend>UDI-DI</legend>
      <div class="field">
        <label for="scheme-select">Scheme</label>
        <select id="scheme-select" v-model="scheme">
          <option value="PPN">PPN (PZN-based)</option>
          <option value="HPC">HPC (Health Product Code)</option>
          <option value="MASTER_UDI_DI">Master UDI-DI</option>
          <option value="AIC">AIC (Italy)</option>
          <option value="AIM">AIM (Portugal)</option>
        </select>
      </div>

      <div v-if="scheme === 'PPN'" class="field-grid">
        <div class="field">
          <label for="pzn-base">PZN base (7 digits, check digit auto-computed)</label>
          <input id="pzn-base" v-model="pznBase" type="text" maxlength="7" placeholder="1234567" />
        </div>
      </div>

      <div v-else-if="scheme === 'HPC'" class="field-grid">
        <div class="field">
          <label for="hpc-cin">CIN (5 chars)</label>
          <input id="hpc-cin" v-model="cin" type="text" maxlength="5" placeholder="12345" />
        </div>
        <div class="field">
          <label for="hpc-item-ref">Item reference (1-18 chars)</label>
          <input id="hpc-item-ref" v-model="itemReference" type="text" maxlength="18" placeholder="MED777" />
        </div>
        <div class="field">
          <label for="hpc-pli">Packaging level index (0-8)</label>
          <input id="hpc-pli" v-model.number="packagingLevelIndex" type="number" min="0" max="8" />
        </div>
      </div>

      <div v-else-if="scheme === 'MASTER_UDI_DI'" class="field-grid">
        <div class="field">
          <label for="mudi-cin">CIN (5 chars)</label>
          <input id="mudi-cin" v-model="cin" type="text" maxlength="5" placeholder="12345" />
        </div>
        <div class="field">
          <label for="mudi-device-group">Device group code (1-19 chars)</label>
          <input id="mudi-device-group" v-model="deviceGroupCode" type="text" maxlength="19" placeholder="MAX19" />
        </div>
      </div>

      <div v-else class="field-grid">
        <div class="field">
          <label for="national-code">National code (1-18 chars, opaque -- IFA does not document an inner format)</label>
          <input id="national-code" v-model="nationalCode" type="text" maxlength="18" placeholder="032220128" />
        </div>
      </div>
    </fieldset>

    <fieldset>
      <legend>UDI-PI (all optional)</legend>
      <div class="field-grid">
        <div class="field">
          <label for="pi-lot">Lot / batch (1-20 chars)</label>
          <input id="pi-lot" v-model="lot" type="text" maxlength="20" />
        </div>
        <div class="field">
          <label for="pi-expiry">Expiry date (YYYY-MM-DD or YYYY-MM)</label>
          <input id="pi-expiry" v-model="expiryDate" type="text" pattern="\d{4}-\d{2}(-\d{2})?" placeholder="2024-12-31" />
        </div>
        <div class="field">
          <label for="pi-mfd">Manufacturing date</label>
          <input id="pi-mfd" v-model="manufacturingDate" type="date" />
        </div>
        <div class="field">
          <label for="pi-serial">Serial number (1-20 chars)</label>
          <input id="pi-serial" v-model="serialNumber" type="text" maxlength="20" />
        </div>
        <div class="field">
          <label for="pi-qty">Quantity</label>
          <input id="pi-qty" v-model.number="quantity" type="number" min="0" />
        </div>
        <div class="field">
          <label for="pi-price">Price (e.g. 12.50)</label>
          <input id="pi-price" v-model="price" type="text" maxlength="20" />
        </div>
        <div class="field">
          <label for="pi-url">URL</label>
          <input id="pi-url" v-model="url" type="text" />
        </div>
        <div class="field">
          <label for="pi-gtins">Additional GTINs (one 14-digit code per line)</label>
          <textarea id="pi-gtins" v-model="additionalGtinsText" rows="2"></textarea>
        </div>
      </div>
    </fieldset>

    <fieldset>
      <legend>Envelope form</legend>
      <div class="field">
        <select id="envelope-form-select" v-model="envelopeForm">
          <option value="interpretationLine">Interpretation Line</option>
          <option value="rawIso15434">Raw ISO/IEC 15434</option>
          <option value="din16598" :disabled="scheme !== 'HPC'">
            DIN 16598 (HPC only{{ scheme !== 'HPC' ? ' — disabled for this scheme' : '' }})
          </option>
        </select>
      </div>
    </fieldset>

    <button type="button" class="primary" @click="generate">Generate barcode</button>

    <template v-if="hasGenerated">
      <div class="two-col" style="margin-top: 1rem">
        <ResultSummary title="TypeScript" :outcome="tsOutcome" />
        <ResultSummary title="C#" :outcome="csharpOutcome" />
      </div>

      <div v-if="'success' in tsOutcome && tsOutcome.success" style="margin-top: 1rem">
        <label>Generated barcode (TypeScript)</label>
        <code class="output-code">{{ tsOutcome.barcode }}</code>
        <button type="button" class="secondary" style="margin-top: 0.5rem" @click="useInCompare">
          Use this barcode in Compare tab
        </button>
      </div>

      <div style="margin-top: 1.5rem">
        <ComparisonTable :left="tsOutcome" :right="csharpOutcome" leftLabel="TypeScript" rightLabel="C#" />
      </div>
    </template>
  </div>
</template>
