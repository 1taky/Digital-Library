import { computed, useAttrs } from 'vue';
import type { BaseIconProps } from './types';

export const useIcon = (props: BaseIconProps, name: string) => {
  const attrs = useAttrs();

  const resolvedTitleId = computed(() => {
    return props.titleId ?? `${name.toLowerCase()}-title`;
  });

  return {
    attrs,
    resolvedTitleId,
  };
};
