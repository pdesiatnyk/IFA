<script setup lang="ts">
import type { ParseOutcome, BuildOutcome } from '../types.js';

const props = defineProps<{ title: string; outcome: ParseOutcome | BuildOutcome }>();
</script>

<template>
  <div
    class="result-card"
    :class="{
      ok: !('loading' in props.outcome) && props.outcome.success,
      error: !('loading' in props.outcome) && !props.outcome.success,
    }"
  >
    <strong>{{ props.title }}</strong>
    <template v-if="'loading' in props.outcome">
      <span> — loading…</span>
    </template>
    <template v-else-if="props.outcome.success">
      <span> — valid</span>
    </template>
    <template v-else>
      <span> — invalid: {{ props.outcome.error.message }}</span>
      <span v-if="'reason' in props.outcome.error" class="badge">{{ props.outcome.error.reason }}</span>
    </template>
  </div>
</template>
