import { cn } from '@/shared/lib/cn';
import { computed } from 'vue';
import type { SectionProps } from './types';

export const useSection = (props: SectionProps) => {
  const className = computed(() => {
    return cn(
      'hidden block w-full min-h-screen items-center justify-center overflow-hidden',
      props.className,
    );
  });
  return { className };
};
