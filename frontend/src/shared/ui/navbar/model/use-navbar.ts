import { computed } from 'vue';

import { cn } from '@/shared/lib/cn';
import type { NavbarProps } from '@/shared/ui/navbar/model/types';

export const useNavbar = (props: NavbarProps) => {
  const className = computed(() => {
    return cn(
      'fixed flex flex-row max-h-[64px] h-full w-full px-32 items-center justify-between bg-background border-b border-muted/10',
      props.className,
    );
  });

  const sideClassName = computed(() => {
    return cn('flex flex-row items-center justify-between');
  });

  const actionClassname = computed(() => {
    return cn(
      'text-muted transition-colors duration-150 ease-in-out hover:text-muted-foreground active:text-foreground',
    );
  });

  return { className, sideClassName, actionClassname };
};
