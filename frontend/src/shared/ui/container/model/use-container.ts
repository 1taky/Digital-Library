import { cn } from '@/shared/lib/cn';
import { computed } from 'vue';
import type { ContainerProps } from './types';

export const useContainer = (props: ContainerProps) => {
  const className = computed(() => {
    return cn(
      'mx-auto w-full px-[24px] md:px-[40px] xl:px-[6.667vw]',
      props.className,
    );
  });
  return { className };
};
