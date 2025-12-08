import { useEffect, useState } from 'react'

interface BreakPoint {
  isMobile: boolean     
  isTablet: boolean    
  isDesktop: boolean  
}

export function useResponsive(): BreakPoint {
  const [breakPoint, setBreakPoint] = useState<BreakPoint>({
    isMobile: window.innerWidth < 768,
    isTablet: window.innerWidth >= 768 && window.innerWidth < 1024,
    isDesktop: window.innerWidth >= 1024,
  })

  useEffect(() => {
    const handleResize = (): void => {
      setBreakPoint({
        isMobile: window.innerWidth < 768,
        isTablet: window.innerWidth >= 768 && window.innerWidth < 1024,
        isDesktop: window.innerWidth >= 1024,
      })
    }

    window.addEventListener('resize', handleResize)
    return () => window.removeEventListener('resize', handleResize)
  }, [])

  return breakPoint
}
