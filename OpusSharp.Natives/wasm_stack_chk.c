#include <stdint.h>

uintptr_t __stack_chk_guard = UINT32_C(0x02135467);

__attribute__((noreturn)) void __stack_chk_fail(void)
{
    __builtin_trap();
    __builtin_unreachable();
}
